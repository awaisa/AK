import { Component, OnInit } from '@angular/core';
import { Http, RequestOptions } from "@angular/http";
import { InventoryService } from './inventory.service';
import { Item } from '../entities/item';
import { Router } from "@angular/router";
import { AppConfiguration } from "../business/appConfiguration";
import { ErrorInfo } from "../shared/ErrorInfo";
import { ErrorDisplayComponent } from '../shared/error-display.component';
import {UserInfo} from "../business/userInfo";

declare var $: any;

@Component({
    templateUrl: './items.component.html'
})
export class ItemsComponent implements OnInit {
    constructor(private http: Http, private router: Router,
        private inventoryService: InventoryService,
        private config: AppConfiguration, private user:UserInfo) {

    }

    itemList: Item[] = [];
    error: ErrorInfo = new ErrorInfo();
    dtOptions: DataTables.Settings = {};
    
    
    ngOnInit(): void {

        this.config.searchText = "";
        this.config.isSearchAllowed = true;
        this.config.activeTab = "inventory";

        setTimeout(() => {
            $("#SearchBox").focus();
        }, 200);

        var t = this.user.token;
        this.dtOptions = {
            ajax: {
                url: this.config.urls.url("items"),
                beforeSend: function(xhr, settings) { 
                    xhr.setRequestHeader('Authorization','Bearer ' + t); 
                } 
            }, 
            serverSide: true,
            ordering: true,
            paging: true,
            pageLength:2,
            lengthChange: false,
            
            columns: [{
              title: 'Code',
              data: 'Code'
            }, {
              title: 'Description',
              data: 'Description'
            }, {
              title: 'Price',
              data: 'Price'
            }]
          };

        //this.getItems();
    }

    getItems() {
        this.inventoryService.getItems()
            .subscribe(items => {
                this.itemList = items;
                console.log("searchtext: " + this.config.searchText);

                setTimeout(() => {
                    $("#MainView").scrollTop(this.inventoryService.listScrollPos);
                    this.inventoryService.listScrollPos = 0;
                }, 20);
                return this.itemList;
            },
            err => { this.error.error(err) }
            );
    }
}
