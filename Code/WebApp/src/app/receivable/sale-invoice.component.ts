import { Component, OnInit } from '@angular/core';
import { slideIn } from '../common/animations';
import { AppConfiguration } from '../business/appConfiguration';
import { BreadcrumbsService } from 'ng2-breadcrumbs';
import { TitleService } from '../shared/title.service';
import { ActivatedRoute } from '@angular/router';
import { RefService } from '../shared/ref.service';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Item, Customer, InvoiceLineItem } from '../entities';
import { ValidationErrorService } from '../shared/validation-error.service';
import { ReceivableService } from './receivable.service';
import { SaleInvoice } from '../entities/saleInvoice';
import { InventoryService } from '../inventory/inventory.service';

declare var $: any;
declare var toastr: any;
declare var window: any;

@Component({
    templateUrl: 'sale-invoice.component.html',
    animations: [slideIn]
  })

export class SaleInvoiceComponent implements OnInit {

  isNew = false;
  isEditMode = false;

  title: string;

  saleForm: FormGroup;

  errors: string[];
  error: ValidationErrorService = new ValidationErrorService();
  loaded = false;
  aniFrame = 'in';

  saleInvoice: SaleInvoice = new SaleInvoice();
  items: Item[] = [];
  customers: Customer[] = [];
  private newLineTime: InvoiceLineItem = new InvoiceLineItem();
  private item: Item = new Item();
    
  constructor(private config: AppConfiguration,
    private route: ActivatedRoute,
    private breadcrumbs: BreadcrumbsService,
    private titleService: TitleService,
    private refService: RefService,
    private fb: FormBuilder,
    private receivableService: ReceivableService,
    private inventroyService: InventoryService)
    {}

  ngOnInit() : void{
    this.isNew = true;
    this.isEditMode = true;
    this.errors = [];
    var id = this.route.snapshot.params["id"];
    this.saleForm = this.fb.group({
      id: [id],
      no:0,
      date:[new Date()],
      description: [''],
      customerId: ['',Validators.required],
      total: 0,
      subTotal: 0,
      taxAmount: 0,
      discountAmount: 0,
      itemId: 0,
      taxGroupId: 0,
      quantity: 0,
      unitPrice: 0,
      itemTotal: 0,
      itemTaxAmount: 0,
      
    })
    this.onChanges();

    this.bindLists();

  }

  onChanges(): void {

    this.title = "Sale Invoice";
    this.titleService.setTitle(this.title);
    // this.itemForm.valueChanges.subscribe(val => {
    //   this.title = `Sale ${val.description == null ? '' : '- ' + val.description }`;
    //   this.titleService.setTitle(this.title);
    // });
  }

  itemOnChange(event: any){
    if( event.target.value != ''){
        this.getNewItemDate(event.target.value);
    }
  }

  getNewItemDate(id){
     
      this.item = new Item();
      this.item = this.items.find(t=> t.id == id);
      let lt = new InvoiceLineItem();
      lt.itemId = this.item.id;
      lt.description = this.item.description;
      lt.unitPrice = this.item.price;
      lt.taxAmount = 0;
      lt.total = 0;
      this.resetLineItem(lt);
  }

  resetLineItem(lt: InvoiceLineItem){

    this.newLineTime = lt;
    this.saleForm.controls['itemId'].setValue(this.newLineTime.itemId);
    this.saleForm.controls['unitPrice'].setValue(this.newLineTime.unitPrice);
    this.saleForm.controls['itemTotal'].setValue(this.newLineTime.total);
    this.saleForm.controls['itemTaxAmount'].setValue(this.newLineTime.taxAmount);
    this.saleForm.controls['quantity'].setValue(this.newLineTime.quantity);
  }

  addLineItem(){

    debugger;

    let oldItem = this.saleInvoice.lineItems.find(t => t.itemId == this.newLineTime.itemId);
    if(oldItem != null){
      var index = this.saleInvoice.lineItems.indexOf(oldItem);
      this.saleInvoice.lineItems[index] = this.newLineTime;
    }
    else{
      this.saleInvoice.lineItems.push(this.newLineTime);
    }
    
    this.resetLineItem(new InvoiceLineItem());
    this.updateSubTotal();
  }

  updateSum(){
    var qty , price, total;
    this.newLineTime.quantity = this.saleForm.value.quantity;
    qty = this.newLineTime.quantity;
    price = this.newLineTime.unitPrice;
    total = qty * price;
    this.newLineTime.total = total;
    this.saleForm.controls['itemTotal'].setValue(this.newLineTime.total);
  }

  updateSubTotal(){
      var subTotal = 0;
      var total = 0;
      var totalTax = 0;
      var totalDiscount = 0;
      for(let lt of this.saleInvoice.lineItems){
        subTotal += lt.total;
        totalTax += lt.taxAmount;
      }
      if(this.saleForm.value.discountAmount > 0){
        this.saleInvoice.discount = this.saleForm.value.discountAmount;
      }

      total = subTotal + totalTax - this.saleInvoice.discount;

      this.saleInvoice.total = total;
      this.saleForm.controls['subTotal'].setValue(subTotal);
      this.saleForm.controls['total'].setValue(total);
  }

  deleteLineItem(id: any){
    this.newLineTime = this.saleInvoice.lineItems.find(t=>t.itemId == id);
    var index = this.saleInvoice.lineItems.indexOf(this.newLineTime);
    this.saleInvoice.lineItems.splice(index,1);
    this.updateSubTotal();
  }
  editLineItem(id: any){
    this.newLineTime =  Object.assign({},this.saleInvoice.lineItems.find(t=>t.itemId == id));
    this.resetLineItem(this.newLineTime);
  }


  isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;
  }

  bindLists(){

    //get customers
      this.refService.getCustomers()
      .subscribe(result => {
          this.customers = result;
      }),
      err => {
          this.error.error(err);
      }

    //get items
    this.refService.getAllItems()
    .subscribe(result => {
        this.items = result;
    }),
    err => {
        this.error.error(err);
    }
  }
}
