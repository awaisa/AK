import { Component, OnInit, ElementRef } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { ErrorInfo } from "../shared/ErrorInfo";
import { AppConfiguration } from "../business/appConfiguration";
import 'rxjs/Rx';

//declare var $:any ;
declare var $: any;
declare var toastr: any;
declare var window: any;

import { slideInLeft, slideIn } from "../common/animations";
import { Brand } from '../entities/brand';
import { resetFakeAsyncZone } from '@angular/core/testing';
import { Catagory } from '../entities/catagory';
import { Model } from '../entities/model';
import { TaxGroup } from '../entities/taxGroup';
import { Measurement } from '../entities/measurement';
import { Account } from '../entities/account';
import { Vendor } from '../entities/vendors';
import { PayableService } from './payable.service';
import { Invoice, InvoiceLineItem } from '../entities/invoice';
import { FormGroup, FormBuilder } from '@angular/forms';
import { InventoryService } from '../inventory/inventory.service';
import { Item } from '../entities/item';
import { Tax } from '../entities/tax';

@Component({
  templateUrl: 'purchase-invoice-edit.html',
  animations: [slideIn]
})
export class InvoiceEditComponent implements OnInit {
  itemForm: FormGroup; 
  successfulSave: boolean;
  errors: string[];

  constructor(private route: ActivatedRoute, private fb: FormBuilder,
    private payableService: PayableService, private inventoryService:InventoryService,
    private config: AppConfiguration) {
  }
  loaded = false;
  error: ErrorInfo = new ErrorInfo();
  invoice: Invoice = new Invoice();
  taxes:Tax[];
  vendors:Vendor[];
  items:Item[];

  //private fieldArray: Array<any> = [];
  private newLineItem: InvoiceLineItem = new InvoiceLineItem();

    ngOnInit(): void {
      this.loaded=true;
      var id = this.route.snapshot.params["id"];
      this.itemForm = this.fb.group({
        brandId: [''],
        code: [''],
        cost: [''],
        costOfGoodsSoldAccountId: [''],
        description: [''],
        id: [id],
        inventoryAccountId: [''],
        itemCategoryId: [''],
        modelId: [''],
        preferredVendorId: [''],
        price: [''],
        purchaseDescription: [''],
        purchaseMeasurementId: [''],
        salesAccountId: [''],
        sellDescription: [''],
        sellMeasurementId: [''],
        smallestMeasurementId: [''],
        taxGroupId: [''],
        date:['']
  
      });
      this.inventoryService.getVendors()
      .subscribe(result => {
        this.vendors = result;
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
      this.inventoryService.getAllItems()
      .subscribe(result => {
        this.items = result;
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
      this.payableService.getTaxes()
            .subscribe(result => {
              this.taxes = result;
              this.loaded = true;
            },
            err => {
              this.error.error(err);
            });
          if(id>0)
            this.payableService.getInvoice(id)
            .subscribe(result => {
              this.invoice = result;
              this.loaded = true;
            },
            err => {
              this.error.error(err);
            });
            
  }

  addFieldValue() {
    this.updateSum(this.newLineItem);
    this.invoice.lineItems.push(this.newLineItem)
    this.newLineItem = new InvoiceLineItem();
    this.SubTotal();
  }

  deleteFieldValue(index) {
    this.invoice.lineItems.splice(index, 1);
      this.SubTotal();
  }

  updateSum(lt: InvoiceLineItem){
    var qty=lt.quantity;
    var price=lt.unitPrice;
    if(qty==null) qty=0;
    if(price==null) price=0;
    lt.total = qty*price;

    //fetch taxes of items and set it to lt.taxes
    //then calculate lt.taxAmount

    this.SubTotal();
  }

  updatePrevSum(index){
    this.updateSum(this.invoice.lineItems[index]);
  }

  SubTotal(){
    var sum=0;
    for (let entry of this.invoice.lineItems)
            sum+=entry.total;
      var total=this.newLineItem.total;
      if(total==null)
          total=0;
        sum+=total;

        this.invoice.subTotal = sum;

        this.invoice.total = this.invoice.subTotal + this.invoice.tax - this.invoice.discount;    
  }

  isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;
  }
}
  

