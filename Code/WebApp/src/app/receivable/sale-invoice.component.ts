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
  isAddItem = false;

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
    this.isNew = false;
    this.isEditMode = false;
    this.errors = [];
    this.isAddItem = true;
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
      
    });

    if (id > 0) {
      this.receivableService.getInvoice(id)
        .subscribe(result => {
          let sl = result;
          this.saleInvoice = sl;
          this.reset();
          this.loaded = true;
        },
        err => {
          this.error.error(err);
        });
    } else {
      this.isEditMode = true;
      this.isNew = true;
    }    

    this.onChanges();

    this.bindLists();

  }

  onChanges(): void {
    this.title = "Sale Invoice";
    //this.titleService.setTitle(this.title);
    this.saleForm.valueChanges.subscribe(val => {
      this.title = `Sale ${val.description == null ? '' : '- ' + val.description }`;
      this.titleService.setTitle(this.title);
    });
  }

  save() {
    this.errors = [];
    if (this.saleForm.valid) {

      if(this.saleInvoice.invoiceItems.length > 0){
      this.saleInvoice.no = this.saleForm.value.no;
      this.saleInvoice.description = this.saleForm.value.description;
      this.saleInvoice.customerId = this.saleForm.value.customerId;
      this.saleInvoice.subTotal = this.saleForm.value.subTotal;
      this.saleInvoice.tax = this.saleForm.value.taxAmount;
      this.saleInvoice.discount = this.saleForm.value.discountAmount;
      

      console.log(this.saleInvoice);
      
      this.receivableService.saveSaleInvoice(this.saleInvoice)
        .subscribe((saleInvoice: SaleInvoice) => {
          var msg = saleInvoice.description + " has been saved."
          this.error.info(msg);
          toastr.success(msg);
          setTimeout(function () {
            //window.location.hash = "inventory/item/" + saleInvoice.id;
          }, 500)
        },
        err => {
          if (err.response.status === 400) {
            // handle validation error
            let validationErrorDictionary = JSON.parse(err.response._body);
            for (var fieldName in validationErrorDictionary) {
              if (this.saleForm.controls[fieldName]) {
                const msg = validationErrorDictionary[fieldName];
                // integrate into angular's validat ion if we have field validation
                this.saleForm.controls[fieldName].setErrors({ error: msg });
              } else {
                // if we have cross field validation then show the validation error at the top of the screen
                this.errors.push(validationErrorDictionary[fieldName]);
              }
            }
          } else {
            this.error.error(err);
          }
        });
      this.saleForm.markAsPristine();
      this.saleForm.markAsUntouched();
      this.saleForm.updateValueAndValidity();      
      }
    }
  }

  
  
  editModeChange(event) {
    this.isEditMode = event;
    if(!event) {
      this.reset();
    }
  }

  reset(){
    console.log(this.saleInvoice);
    
    this.saleForm.reset({
      id: this.saleInvoice.id,
      no:this.saleInvoice.no,
      date:this.saleInvoice.date,
      description: this.saleInvoice.description,
      customerId: this.saleInvoice.customerId,
      total: this.saleInvoice.total,
      subTotal: this.saleInvoice.subTotal,
      taxAmount: 0,
      discountAmount: 0,
    });
    for(var lt of this.saleInvoice.invoiceItems){
      let item = this.items.find(t=> t.id == lt.itemId);
      if(item != null){
        lt.description = item.description ;
        lt.unitPrice = lt.amount;
        lt.total = lt.quantity * lt.amount;
      }
    }
    this.updateSubTotal();
  }

  itemOnChange(event: any){
    if(event.target.value != ''){
        this.getNewItemDate(event.target.value);
    }
    else{
      this.resetLineItem(new InvoiceLineItem());
    }
  }

  getNewItemDate(id){
     
      this.item = new Item();
      this.item = this.items.find(t=> t.id == id);
      let lt = new InvoiceLineItem();
      lt.itemId = this.item.id;
      lt.measurementId = this.item.sellMeasurementId;
      lt.description = this.item.description;
      lt.unitPrice = this.item.price;
      lt.amount = this.item.price;
      lt.taxAmount = 0;
      lt.total = 0;
      this.resetLineItem(lt);
  }

  resetLineItem(lt: InvoiceLineItem){
    
    this.isAddItem = true;
    this.newLineTime = lt;
    this.saleForm.controls['itemId'].setValue(this.newLineTime.itemId);
    this.saleForm.controls['unitPrice'].setValue(this.newLineTime.unitPrice);
    this.saleForm.controls['itemTotal'].setValue(this.newLineTime.total);
    this.saleForm.controls['itemTaxAmount'].setValue(this.newLineTime.taxAmount);
    this.saleForm.controls['quantity'].setValue(this.newLineTime.quantity);
  }

  addLineItem(){

    debugger;

    let oldItem = this.saleInvoice.invoiceItems.find(t => t.itemId == this.newLineTime.itemId);
    if(oldItem != null){
      var index = this.saleInvoice.invoiceItems.indexOf(oldItem);
      this.saleInvoice.invoiceItems[index] = this.newLineTime;
    }
    else{
      this.saleInvoice.invoiceItems.push(this.newLineTime);
    }
    
    this.resetLineItem(new InvoiceLineItem());
    this.updateSubTotal();
  }

  updateSum(){
    var qty , price, total;
    if(this.saleForm.value.quantity != 0 && this.saleForm.value.itemId != 0){
      this.isAddItem = false;
      qty = this.newLineTime.quantity;
      this.newLineTime.quantity = this.saleForm.value.quantity;
      price = this.newLineTime.unitPrice;
      total = qty * price;
      this.newLineTime.total = total;
      this.saleForm.controls['itemTotal'].setValue(this.newLineTime.total);
    }
    else{
      this.isAddItem = true;
    }
  }

  updateSubTotal(){
    debugger;
      var subTotal = 0;
      var total = 0;
      var totalTax = 0;
      var totalDiscount = 0;
      for(let lt of this.saleInvoice.invoiceItems){
        subTotal += lt.total;
        //totalTax += lt.taxAmount;
      }
      if(this.saleForm.value.discountAmount > 0){
        this.saleInvoice.discount = this.saleForm.value.discountAmount;
      }
      else{
        this.saleInvoice.discount = 0;
      }
      

      total = subTotal + totalTax - this.saleInvoice.discount;

      this.saleInvoice.total = total;
      this.saleForm.controls['subTotal'].setValue(subTotal);
      this.saleForm.controls['total'].setValue(total);
  }

  deleteLineItem(id: any){
    this.newLineTime = this.saleInvoice.invoiceItems.find(t=>t.itemId == id);
    var index = this.saleInvoice.invoiceItems.indexOf(this.newLineTime);
    this.saleInvoice.invoiceItems.splice(index,1);
    this.updateSubTotal();
  }
  editLineItem(id: any){
    this.newLineTime =  Object.assign({},this.saleInvoice.invoiceItems.find(t=>t.itemId == id));
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
