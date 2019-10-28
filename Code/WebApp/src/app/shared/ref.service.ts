import { Injectable } from '@angular/core';
import { Item } from "../entities/item";
import {AppConfiguration} from "../business/appConfiguration";
import { HttpClientService } from "../business/http-client.service";
import { ValidationErrorService } from "../shared/validation-error.service";
import { Observable } from "rxjs";
import { Brand, Catagory, Model, TaxGroup, Measurement, Account, Vendor, PaymentTerm, Customer } from '../entities';


@Injectable()
export class RefService {
    
    itemList: Item[] = [];
    catagoryList:Catagory[]=[];
    brandList:Brand[]=[];
    modelList:Model[]=[];
    taxGroupList:TaxGroup[]=[];
    measurementList:Measurement[]=[];
    accountList:Account[]=[];
    vendorList:Vendor[]=[];
    paymentTermList:PaymentTerm[]=[];
    customerList:Customer[]=[];

    constructor(private httpClient: HttpClientService,
        private config: AppConfiguration) {
    }

    getAllItems(force: boolean = false): Observable<Item[]> {

        // use locally cached version
        if (force !== true && (this.itemList && this.itemList.length > 0))
            return Observable.of(this.itemList) as Observable<Item[]>;

        return this.httpClient.get(this.config.urls.url("AllItems"))
            .map(response => {
                this.itemList = response.json();

                return this.itemList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);
    }
    
    getBrands(force: boolean = false): Observable<Brand[]> {

        // use locally cached version
        if (force !== true && (this.brandList && this.brandList.length > 0))
            return Observable.of(this.brandList) as Observable<Brand[]>;

        return this.httpClient.get(this.config.urls.url("brands"))
            .map(response => {
                this.brandList = response.json();

                return this.brandList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);
    }
   
    getCatagories(force: boolean = false): Observable<Catagory[]> {

        // use locally cached version
        if (force !== true && (this.catagoryList && this.catagoryList.length > 0))
            return Observable.of(this.catagoryList) as Observable<Catagory[]>;

        return this.httpClient.get(this.config.urls.url("catagories"))
            .map(response => {
                this.catagoryList = response.json();

                return this.catagoryList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);

    }

    getModels(force: boolean = false): Observable<Model[]> {

        // use locally cached version
        if (force !== true && (this.modelList && this.modelList.length > 0))
            return Observable.of(this.modelList) as Observable<Model[]>;

        return this.httpClient.get(this.config.urls.url("models"))
            .map(response => {
                this.modelList = response.json();

                return this.modelList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);

    }

    getTaxGroups(force: boolean = false): Observable<TaxGroup[]> {

        // use locally cached version
        if (force !== true && (this.taxGroupList && this.taxGroupList.length > 0))
            return Observable.of(this.taxGroupList) as Observable<TaxGroup[]>;

        return this.httpClient.get(this.config.urls.url("taxgroups"))
            .map(response => {
                this.taxGroupList = response.json();

                return this.taxGroupList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);

    }
    getMeasurements(force: boolean = false): Observable<Measurement[]> {

        // use locally cached version
        if (force !== true && (this.measurementList && this.measurementList.length > 0))
            return Observable.of(this.measurementList) as Observable<Measurement[]>;

        return this.httpClient.get(this.config.urls.url("measuremets"))
            .map(response => {
                this.measurementList = response.json();

                return this.measurementList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);

    }
    getAccounts(force: boolean = false): Observable<Account[]> {

        // use locally cached version
        if (force !== true && (this.accountList && this.accountList.length > 0))
            return Observable.of(this.accountList) as Observable<Account[]>;

        return this.httpClient.get(this.config.urls.url("accounts"))
            .map(response => {
                this.accountList = response.json();

                return this.accountList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);

    }

    getVendors(force: boolean = false): Observable<Vendor[]> {

        // use locally cached version
        if (force !== true && (this.vendorList && this.vendorList.length > 0))
            return Observable.of(this.vendorList) as Observable<Vendor[]>;

        return this.httpClient.get(this.config.urls.url("vendorss"))
            .map(response => {
                this.vendorList = response.json();

                return this.vendorList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);

    }
    getPaymentTerms(force: boolean = false): Observable<PaymentTerm[]> {

        // use locally cached version
        if (force !== true && (this.paymentTermList && this.paymentTermList.length > 0))
            return Observable.of(this.paymentTermList) as Observable<PaymentTerm[]>;

        return this.httpClient.get(this.config.urls.url("paymentTerm"))
            .map(response => {
                this.paymentTermList = response.json();

                return this.paymentTermList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);

    }
    getCustomers(force: boolean = false): Observable<Customer[]>{
         // use locally cached version
         if (force !== true && (this.customerList && this.customerList.length > 0))
         return Observable.of(this.customerList) as Observable<Customer[]>;

     return this.httpClient.get(this.config.urls.url("getCustomers"))
         .map(response => {
             this.customerList = response.json();

             return this.customerList;
         })
         .catch(new ValidationErrorService().parseObservableResponseError);
    }
}
