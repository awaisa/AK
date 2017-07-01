import { Component, OnInit } from '@angular/core';
import { Http, RequestOptions } from "@angular/http";
import { InventoryService } from './inventory.service';
import { Item } from '../entities/item';
import { Router } from "@angular/router";
import { AppConfiguration } from "../business/appConfiguration";
import { ErrorInfo } from "../shared/ErrorInfo";
import { ErrorDisplayComponent } from '../shared/error-display.component';
import { Subject } from 'rxjs/Rx';

declare var $: any;

@Component({
    templateUrl: './items.component.html'
})
export class ItemsComponent implements OnInit {
    constructor(private http: Http, private router: Router,
        private inventoryService: InventoryService,
        private config: AppConfiguration) {

    }

    itemList: Item[] = [];
    error: ErrorInfo = new ErrorInfo();

    ngOnInit() {
        this.getItems();

        this.config.searchText = "";
        this.config.isSearchAllowed = true;
        this.config.activeTab = "inventory";

        setTimeout(() => {
            $("#SearchBox").focus();
        }, 200);
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
