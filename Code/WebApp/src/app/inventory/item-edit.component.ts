import { Component, OnInit, ElementRef } from '@angular/core';
import { Item } from '../entities/item';
import { InventoryService } from './inventory.service';
import { ActivatedRoute } from "@angular/router";
import { ValidationErrorService } from "../shared/validation-error.service";
import { AppConfiguration } from "../business/appConfiguration";
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import 'rxjs/Rx';

import { slideInLeft, slideIn } from "../common/animations";
import { resetFakeAsyncZone } from '@angular/core/testing';
import { Brand, Catagory, Model, TaxGroup, Measurement, Account, Vendor } from '../entities';
import { RefService } from '../shared/ref.service';
import { TitleService } from '../shared/title.service';

declare var $: any;
declare var toastr: any;
declare var window: any;

@Component({
  templateUrl: 'item-edit.component.html',
  animations: [slideIn]
})
export class ItemEditComponent implements OnInit {

  isNew = false;
  isEditMode = false;

  title: string;

  itemForm: FormGroup;
  item: Item = new Item();
  errors: string[];
  error: ValidationErrorService = new ValidationErrorService();
  loaded = false;
  aniFrame = 'in';

  brands: Brand[] = [];
  catagories: Catagory[] = [];
  models: Model[] = [];
  taxgroups: TaxGroup[] = [];
  measurements: Measurement[] = [];
  accounts: Account[] = [];
  vendors: Vendor[] = [];

  constructor(private route: ActivatedRoute, private fb: FormBuilder,
    private inventoryService: InventoryService, private refService: RefService,
    private config: AppConfiguration,
    private titleService: TitleService) {
  }

  ngOnInit() {    

    this.errors = [];
    var id = this.route.snapshot.params["id"];
    this.itemForm = this.fb.group({
      id: [id],
      code: ['', Validators.required],
      cost: ['', Validators.required],
      price: ['', Validators.required],
      description: ['', Validators.required],
      purchaseDescription: [''],
      sellDescription: [''],
      itemCategoryId: ['', Validators.required],
      modelId: ['', Validators.required],
      brandId: [''],
      purchaseMeasurementId: [''],
      sellMeasurementId: [''],
      smallestMeasurementId: [''],
      taxGroupId: [''],
      preferredVendorId: ['', Validators.required],
      costOfGoodsSoldAccountId: ['', Validators.required],
      inventoryAccountId: ['', Validators.required],
      salesAccountId: [''],
    });
    
    this.bindLists();

    if (id > 0) {
      this.inventoryService.getItem(id)
        .subscribe(result => {
          this.item = result;
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
  }
  
  onChanges(): void {
    this.itemForm.valueChanges.subscribe(val => {
      this.title = `Item ${val.description == null ? '' : '- ' +val.description }`;
      this.titleService.setTitle(this.title);
    });
  }

  editModeChange(event) {
    this.isEditMode = event;
    if(!event) {
      this.reset();
    }
  }

  save() {
    this.errors = [];
    if (this.itemForm.valid) {
      let item = {
        id: this.item.id,
        brandId: this.itemForm.value.brandId,
        code: this.itemForm.value.code,
        cost: this.itemForm.value.cost,
        costOfGoodsSoldAccountId: this.itemForm.value.costOfGoodsSoldAccountId,
        description: this.itemForm.value.description,
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
      this.inventoryService.saveItem(item)
        .subscribe((item: Item) => {
          var msg = item.description + " has been saved."
          this.error.info(msg);
          toastr.success(msg);
          setTimeout(function () {
            window.location.hash = "inventory/item/" + item.id;
          }, 500)
        },
        err => {
          if (err.response.status === 400) {
            // handle validation error
            let validationErrorDictionary = JSON.parse(err.response._body);
            for (var fieldName in validationErrorDictionary) {
              if (this.itemForm.controls[fieldName]) {
                const msg = validationErrorDictionary[fieldName];
                // integrate into angular's validat ion if we have field validation
                this.itemForm.controls[fieldName].setErrors({ error: msg });
              } else {
                // if we have cross field validation then show the validation error at the top of the screen
                this.errors.push(validationErrorDictionary[fieldName]);
              }
            }
          } else {
            this.error.error(err);
          }
        });
      this.itemForm.markAsPristine();
      this.itemForm.markAsUntouched();
      this.itemForm.updateValueAndValidity();      
    }
  }

  reset(){
    this.itemForm.reset({
      code: this.item.code,
      cost: this.item.cost,
      price: this.item.price,
      description: this.item.description,
      purchaseDescription: this.item.purchaseDescription,
      sellDescription: this.item.sellDescription,
      itemCategoryId: this.item.itemCategoryId,
      modelId: this.item.modelId,
      brandId: this.item.brandId,
      sellMeasurementId: this.item.sellMeasurementId,
      smallestMeasurementId: this.item.smallestMeasurementId,
      purchaseMeasurementId: this.item.purchaseMeasurementId,
      taxGroupId: this.item.taxGroupId,
      preferredVendorId: this.item.preferredVendorId,
      costOfGoodsSoldAccountId: this.item.costOfGoodsSoldAccountId,
      inventoryAccountId: this.item.inventoryAccountId,
      salesAccountId: this.item.salesAccountId,            
    });
  }

  bindLists(){
    this.refService.getBrands()
    .subscribe(result => {
      this.brands = result;
    },
    err => {
      this.error.error(err);
    });
    this.refService.getCatagories()
      .subscribe(result => {
        this.catagories = result;
      },
      err => {
        this.error.error(err);
      });
    this.refService.getModels()
      .subscribe(result => {
        this.models = result;
      },
      err => {
        this.error.error(err);
      });
    this.refService.getMeasurements()
      .subscribe(result => {
        this.measurements = result;
      },
      err => {
        this.error.error(err);
      });
    this.refService.getTaxGroups()
      .subscribe(result => {
        this.taxgroups = result;
      },
      err => {
        this.error.error(err);
      });
    this.refService.getAccounts()
      .subscribe(result => {
        this.accounts = result;
      },
      err => {
        this.error.error(err);
      });
    this.refService.getVendors()
      .subscribe(result => {
        this.vendors = result;
      },
      err => {
        this.error.error(err);
      });
  }
}
