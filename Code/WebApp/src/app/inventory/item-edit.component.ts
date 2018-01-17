import {Component, OnInit, ElementRef} from '@angular/core';
import { Item } from '../entities/item';
import { InventoryService } from './inventory.service';
import { ActivatedRoute } from "@angular/router";
import { ErrorInfo } from "../shared/ErrorInfo";
import { AppConfiguration } from "../business/appConfiguration";

//declare var $:any ;
declare var $:any;
declare var toastr:any;
declare var window:any;

import {slideInLeft, slideIn} from "../common/animations";
import { Brand } from '../entities/brand';
import { resetFakeAsyncZone } from '@angular/core/testing';
import { Catagory } from '../entities/catagory';
import { Model } from '../entities/model';
import { TaxGroup } from '../entities/taxGroup';
import { Measurement } from '../entities/measurement';
import { Account } from '../entities/account';

@Component({
    templateUrl: 'item-edit.component.html',
    animations: [ slideIn ]
})
export class ItemEditComponent implements OnInit {
  constructor(  private route: ActivatedRoute,
                private inventoryService: InventoryService,
                private config: AppConfiguration) {
  }

  item: Item = new Item();
  error: ErrorInfo = new ErrorInfo();
  loaded =  false;
  aniFrame = 'in';

  brands:Brand[]=[];
  catagories:Catagory[]=[];
  models:Model[]=[];
  taxgroups:TaxGroup[]=[];
  measurements:Measurement[]=[];
  accounts:Account[]=[];



  ngOnInit() {
    
    //this.config.isSearchAllowed = false;
    //this.bandTypeAhead();

    var id = this.route.snapshot.params["id"];
    if (id < 1) {
      this.loaded = true;
      return;
    }
    this.inventoryService.getBrands()
      .subscribe(result=>{
        this.brands=result;
        this.loaded=true;
      },
      err => {
        this.error.error(err);
      });

      this.inventoryService.getCatagories()
      .subscribe(result=>{
        this.catagories=result;
        this.loaded=true;
      },
      err => {
        this.error.error(err);
      });

      this.inventoryService.getModels()
      .subscribe(result=>{
        this.models=result;
        this.loaded=true;
      },
      err => {
        this.error.error(err);
      });

      this.inventoryService.getMeasurements()
      .subscribe(result=>{
        this.measurements=result;
        this.loaded=true;
      },
      err => {
        this.error.error(err);
      });

      this.inventoryService.getTaxGroups()
      .subscribe(result=>{
        this.taxgroups=result;
        this.loaded=true;
      },
      err => {
        this.error.error(err);
      });

      this.inventoryService.getAccounts()
      .subscribe(result=>{
        this.accounts=result;
        this.loaded=true;
      },
      err => {
        this.error.error(err);
      });

    this.inventoryService.getItem(id)
      .subscribe(result => {
          this.item = result;
          this.loaded = true;
        },
        err => {
          this.error.error(err);
        });

        
  }

  saveItem(item) {
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
         let msg = `Unable to save item: ${err.message}`;
         this.error.error(msg);
         toastr.error(msg);


         //if (err.response && err.response.status == 401) {
         //  this.user.isAuthenticated = false;
         //  window.location.hash = "login";
         //}
       });

  };

  bandTypeAhead() {
    var $input:any = $("#BandName");
    var config = this.config;

    // delay slightly to ensure that the
    // typeahead component is loaded when
    // doing a full browser refresh
    setTimeout( function () {
        $input.typeahead({
            source: [],
            autoselect: true,
            minLength: 0
        });

        $input.keyup( function() {
          let s = $(this).val();
          let url = config.urls.url("artistLookup") + s;

          $.getJSON(url,
              (data) => {
                  $input.data('typeahead').source = data;
              });
        });

    },1000);

    }
}
