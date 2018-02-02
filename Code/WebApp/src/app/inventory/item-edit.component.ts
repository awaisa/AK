import { Component, OnInit, ElementRef } from '@angular/core';
import { Item } from '../entities/item';
import { InventoryService } from './inventory.service';
import { ActivatedRoute } from "@angular/router";
import { ErrorInfo } from "../shared/ErrorInfo";
import { AppConfiguration } from "../business/appConfiguration";
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
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

@Component({
  templateUrl: 'item-edit.component.html',
  animations: [slideIn]
})
export class ItemEditComponent implements OnInit {
  itemForm: FormGroup;
  successfulSave: boolean;
  errors: string[];

  constructor(private route: ActivatedRoute, private fb: FormBuilder,
    private inventoryService: InventoryService,
    private config: AppConfiguration) {
  }
  item: Item = new Item();
  error: ErrorInfo = new ErrorInfo();
  loaded = false;
  aniFrame = 'in';

  brands: Brand[] = [];
  catagories: Catagory[] = [];
  models: Model[] = [];
  taxgroups: TaxGroup[] = [];
  measurements: Measurement[] = [];
  accounts: Account[] = [];
  vendors: Vendor[] = [];

  ngOnInit() {
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
      taxGroupId: ['']

    });
    this.errors = [];
    //this.config.isSearchAllowed = false;
    //this.bandTypeAhead();
    if (id < 0) {
      this.loaded = true;
      return;
    }
    this.inventoryService.getBrands()
      .subscribe(result => {
        this.brands = result;
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
    this.inventoryService.getCatagories()
      .subscribe(result => {
        this.catagories = result;
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
    this.inventoryService.getModels()
      .subscribe(result => {
        this.models = result;
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
    this.inventoryService.getMeasurements()
      .subscribe(result => {
        this.measurements = result;
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
    this.inventoryService.getTaxGroups()
      .subscribe(result => {
        this.taxgroups = result;
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
    this.inventoryService.getAccounts()
      .subscribe(result => {
        this.accounts = result;
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
    this.inventoryService.getVendors()
      .subscribe(result => {
        this.vendors = result;
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
    if (id > 0)
      this.inventoryService.getItem(id)
        .subscribe(result => {
          this.item = result;
          this.loaded = true;
        },
        err => {
          this.error.error(err);
        });
  }
  saveItem() {
    this.errors = [];
    if (this.itemForm.valid) {
      let item = {
        brandId: this.itemForm.value.brandId,
        code: this.itemForm.value.code,
        cost: this.itemForm.value.cost,
        costOfGoodsSoldAccountId: this.itemForm.value.costOfGoodsSoldAccountId,
        description: this.itemForm.value.description,
        id: this.itemForm.value.id,
        inventoryAccountId: this.itemForm.value.inventoryAccountId,
        itemCategoryId: this.itemForm.value.itemCategoryId,
        modelId: this.itemForm.value.modelId,
        preferredVendorId: this.itemForm.value.preferredVendorId,
        price: this.itemForm.value.price,
        purchaseDescription: this.itemForm.value.purchaseDescription,
        purchaseMeasurementId: this.itemForm.value.purchaseMeasurementId,
        salesAccountId: this.itemForm.value.salesAccountId,
        sellDescription: this.itemForm.value.sellDescription,
        sellMeasurementId: this.itemForm.value.sellMeasurementId,
        smallestMeasurementId: this.itemForm.value.smallestMeasurementId,
        taxGroupId: this.itemForm.value.taxGroupId,
      };
      return this.inventoryService.saveItem(item)
        .subscribe((item: Item) => {
          var msg = item.description + " has been saved."
          this.error.info(msg);
          toastr.success(msg);
          window.document.getElementById("MainView").scrollTop = 0;

          setTimeout(function () {
            window.location.hash = "inventory/item/" + item.id;
          }, 1500)
        },
        err => {
          this.successfulSave = false;
          if (err.response.status === 400) {
            // handle validation error
            let validationErrorDictionary = JSON.parse(err.response._body);
            for (var fieldName in validationErrorDictionary) {
              if (this.itemForm.controls[fieldName]) {
                // integrate into angular's validat ion if we have field validation
                this.itemForm.controls[fieldName].setErrors({ invalid: true });
                this.errors.push(validationErrorDictionary[fieldName]);
              } else {
                // if we have cross field validation then show the validation error at the top of the screen
                this.errors.push(validationErrorDictionary[fieldName]);
              }
            }
          } else {
            this.errors.push("something went wrong!");
          }
          //  let msg = `Unable to save item: ${err.message}`;
          //  this.error.error(msg);
          //  toastr.error(msg);

          //if (err.response && err.response.status == 401) {
          //  this.user.isAuthenticated = false;
          //  window.location.hash = "login";
          //}
        });
    }
  };
  bandTypeAhead() {
    var $input: any = $("#BandName");
    var config = this.config;

    // delay slightly to ensure that the
    // typeahead component is loaded when
    // doing a full browser refresh
    setTimeout(function () {
      $input.typeahead({
        source: [],
        autoselect: true,
        minLength: 0
      });

      $input.keyup(function () {
        let s = $(this).val();
        let url = config.urls.url("artistLookup") + s;

        $.getJSON(url,
          (data) => {
            $input.data('typeahead').source = data;
          });
      });

    }, 1000);

  }
}
