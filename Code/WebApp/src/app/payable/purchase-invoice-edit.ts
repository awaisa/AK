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
import { TaxGroupTax } from '../entities/taxGroupTax';

@Component({
  templateUrl: 'purchase-invoice-edit.html',
  animations: [slideIn]
})
export class InvoiceEditComponent implements OnInit {
  itemForm: FormGroup;
  successfulSave: boolean;
  errors: string[];

  constructor(private route: ActivatedRoute, private fb: FormBuilder,
    private payableService: PayableService, private inventoryService: InventoryService,
    private config: AppConfiguration) {
  }
  loaded = false;
  error: ErrorInfo = new ErrorInfo();
  invoice: Invoice = new Invoice();
  taxes: Tax[];
  taxGroups: TaxGroup[];
  taxGroupTaxes: TaxGroupTax[];
  vendors: Vendor[];
  items: Item[];
  taxLineItem: InvoiceLineItem = new InvoiceLineItem();
  //private fieldArray: Array<any> = [];
  private newLineItem: InvoiceLineItem = new InvoiceLineItem();

  ngOnInit(): void {
    this.loaded = true;
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
      date: ['']

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
        for (let entry of this.taxes)
          this.taxLineItem.taxes.push(entry);
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
    this.payableService.getTaxGroupTax()
      .subscribe(result => {
        this.taxGroupTaxes = result;
        for (let entry of this.taxGroupTaxes)
          this.taxLineItem.taxGroupTaxes.push(entry);
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
    if (id > 0)
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
  updateSum(lt: InvoiceLineItem) {
    var taxGroupArr, taxitem, taxRate = 0, qty, price;
    qty = lt.quantity;
    price = lt.unitPrice;
    if (qty == null) qty = 0;
    if (price == null) price = 0;
    lt.total = qty * price;
    for (let field of this.items) {
      if (field.id == lt.itemId) {
        lt.taxGroupId = field.taxGroupId;
        break;
      }
    }
    taxGroupArr = this.taxLineItem.taxGroupTaxes.filter(groupid => groupid.taxGroupId == lt.taxGroupId);
    for (let taxGroup of taxGroupArr) {
      taxitem = this.taxLineItem.taxes.filter(tax => tax.id == taxGroup.taxId);
      for (let field of taxitem)
        taxRate += field.rate;
    }
    lt.taxAmount = (lt.total * taxRate) / 100;
    this.SubTotal();
  }
  updatePrevSum(index) {
    this.updateSum(this.invoice.lineItems[index]);
  }
  SubTotal() {
    var sum = 0;
    var taxSum = 0;
    for (let entry of this.invoice.lineItems) {
      sum += entry.total;
      taxSum += entry.taxAmount;
    }
    var tax = this.newLineItem.taxAmount;
    taxSum += tax;
    var total = this.newLineItem.total;
    if (total == null)
      total = 0;
    sum += total;
    this.invoice.subTotal = sum;
    this.invoice.tax = taxSum;
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


