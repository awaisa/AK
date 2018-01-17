import { Injectable } from '@angular/core';
import { Item } from "../entities/item";
import {AppConfiguration} from "../business/appConfiguration";
import { HttpClientService } from "../business/http-client.service";
import { ErrorInfo } from "../shared/ErrorInfo";
import { Observable } from "rxjs";
import { Brand } from '../entities/brand';
import { Catagory } from '../entities/catagory';
import {  Account } from '../entities/account';
import { TaxGroup } from '../entities/taxGroup';
import { Measurement } from '../entities/measurement';
import { Model } from '../entities/model';

@Injectable()
export class InventoryService {
    constructor(private httpClient: HttpClientService,
        private config: AppConfiguration) {
        //console.log("InventoryService ctor");
    }

    itemList: Item[] = [];
    item: Item = null;
    error: string = "";

    listScrollPos = 0;

    getItems(force: boolean = false): Observable<Item[]> {

        // use locally cached version
        if (force !== true && (this.itemList && this.itemList.length > 0))
            return Observable.of(this.itemList) as Observable<Item[]>;

        return this.httpClient.get(this.config.urls.url("items"))
            .map(response => {
                this.itemList = response.json();

                return this.itemList;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
    }

    brandList:Brand[]=[];
    getBrands(force: boolean = false): Observable<Brand[]> {

        // use locally cached version
        if (force !== true && (this.brandList && this.brandList.length > 0))
            return Observable.of(this.brandList) as Observable<Brand[]>;

        return this.httpClient.get(this.config.urls.url("brands"))
            .map(response => {
                this.brandList = response.json();

                return this.brandList;
            })
            .catch(new ErrorInfo().parseObservableResponseError);

    }
   

    catagoryList:Catagory[]=[];
    getCatagories(force: boolean = false): Observable<Catagory[]> {

        // use locally cached version
        if (force !== true && (this.catagoryList && this.catagoryList.length > 0))
            return Observable.of(this.catagoryList) as Observable<Catagory[]>;

        return this.httpClient.get(this.config.urls.url("catagories"))
            .map(response => {
                this.catagoryList = response.json();

                return this.catagoryList;
            })
            .catch(new ErrorInfo().parseObservableResponseError);

    }

    modelList:Model[]=[];
    getModels(force: boolean = false): Observable<Model[]> {

        // use locally cached version
        if (force !== true && (this.modelList && this.modelList.length > 0))
            return Observable.of(this.modelList) as Observable<Model[]>;

        return this.httpClient.get(this.config.urls.url("models"))
            .map(response => {
                this.modelList = response.json();

                return this.modelList;
            })
            .catch(new ErrorInfo().parseObservableResponseError);

    }

    taxGroupList:TaxGroup[]=[];
    getTaxGroups(force: boolean = false): Observable<TaxGroup[]> {

        // use locally cached version
        if (force !== true && (this.taxGroupList && this.taxGroupList.length > 0))
            return Observable.of(this.taxGroupList) as Observable<TaxGroup[]>;

        return this.httpClient.get(this.config.urls.url("taxgroups"))
            .map(response => {
                this.taxGroupList = response.json();

                return this.taxGroupList;
            })
            .catch(new ErrorInfo().parseObservableResponseError);

    }
    measurementList:Measurement[]=[];
    getMeasurements(force: boolean = false): Observable<Measurement[]> {

        // use locally cached version
        if (force !== true && (this.measurementList && this.measurementList.length > 0))
            return Observable.of(this.measurementList) as Observable<Measurement[]>;

        return this.httpClient.get(this.config.urls.url("measuremets"))
            .map(response => {
                this.measurementList = response.json();

                return this.measurementList;
            })
            .catch(new ErrorInfo().parseObservableResponseError);

    }
    accountList:Account[]=[];
    getAccounts(force: boolean = false): Observable<Account[]> {

        // use locally cached version
        if (force !== true && (this.accountList && this.accountList.length > 0))
            return Observable.of(this.accountList) as Observable<Account[]>;

        return this.httpClient.get(this.config.urls.url("accounts"))
            .map(response => {
                this.accountList = response.json();

                return this.accountList;
            })
            .catch(new ErrorInfo().parseObservableResponseError);

    }


    getItem(id) {
        return this.httpClient.get(this.config.urls.url("items", id))
            .map(response => {
                var result = response.json();
                this.item = result;

                if (!this.itemList || this.itemList.length < 1)
                    this.getItems();

                return result;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
    }

    saveItem(item): Observable<any> {
        return this.httpClient.post(this.config.urls.url("items"), item, null)
            .map(response => {
                this.item = response.json();
                
                // explicitly update the list with the updated data
                this.updateItem(this.item);
                return this.item;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
    }

    /**
     * Updates the .albumList property by updating the actual
     * index entry in the existing list, adding new entries and
     * removing 0 entries.
     * @param item  - the item to update
     */
    updateItem(item) {
        var i = this.itemList.findIndex((a) => (a.id == item.Id));
        if (i > -1)
            this.itemList[i] = item;
        else {
            this.itemList.push(item);
            this.itemList.sort((a: Item, b: Item) => {
                var aCode = a.code.toLocaleLowerCase();
                var bCode = b.code.toLocaleLowerCase();
                if (aCode > bCode)
                    return 1;
                if (aCode < bCode)
                    return -1;
                return 0;
            })
        }

        this.itemList = this.itemList.filter((a) => a.id != 0);
    }
}
