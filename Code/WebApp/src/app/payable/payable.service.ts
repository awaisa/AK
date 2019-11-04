import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
// import { HttpClientService } from '../business/http-client.service';
import { AppConfiguration } from '../business/appConfiguration';
import { Observable } from 'rxjs';
import { Invoice } from '../entities/invoice';
import { ValidationErrorService } from '../shared/validation-error.service';
import { Tax } from '../entities/tax';
import { TaxGroup } from '../entities/taxGroup';
import { TaxGroupTax } from '../entities/taxGroupTax';

@Injectable()
export class PayableService {

  constructor(private httpClient: HttpClient,
    private config: AppConfiguration) {
}
invoiceList: Invoice[] = [];
taxList:Tax[]=[];
taxGroupList:TaxGroup[]=[];
taxGroupTaxList:TaxGroupTax[]=[];
error: string = "";

listScrollPos = 0;

getInvoices(force: boolean = false): Observable<Invoice[]> {

    // use locally cached version
    if (force !== true && (this.invoiceList && this.invoiceList.length > 0))
        return Observable.of(this.invoiceList) as Observable<Invoice[]>;

        return this.httpClient.get<Invoice[]>(this.config.urls.url("purchaseInvoices"))
            .map(response => {
                this.invoiceList = response;
                
                return this.invoiceList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError); 
 }

 getInvoice(id) {
  return this.httpClient.get<Invoice>(this.config.urls.url("purchaseInvoices", id))
      .map(response => {
          return response;
          
          //return this.invoiceList;
      })
      .catch(new ValidationErrorService().parseObservableResponseError);
}
getTaxes(force: boolean = false): Observable<Tax[]> {

    // use locally cached version
    if (force !== true && (this.taxList && this.taxList.length > 0))
        return Observable.of(this.taxList) as Observable<Tax[]>;

        return this.httpClient.get<Tax[]>(this.config.urls.url("Taxes"))
            .map(response => {
                this.taxList = response;
                
                return this.taxList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);
 }
 getTaxGroupTax(force: boolean = false): Observable<TaxGroupTax[]> {

    // use locally cached version
    if (force !== true && (this.taxGroupTaxList && this.taxGroupTaxList.length > 0))
        return Observable.of(this.taxGroupTaxList) as Observable<TaxGroupTax[]>;

        return this.httpClient.get<TaxGroupTax[]>(this.config.urls.url("Taxgrouptax"))
            .map(response => {
                this.taxGroupTaxList = response;
                
                return this.taxGroupTaxList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);
 }

 getTaxGroup(force: boolean = false): Observable<TaxGroup[]> {

    // use locally cached version
    if (force !== true && (this.taxGroupList && this.taxGroupList.length > 0))
        return Observable.of(this.taxGroupList) as Observable<TaxGroup[]>;

        return this.httpClient.get<TaxGroup[]>(this.config.urls.url("TaxGroup"))
            .map(response => {
                this.taxGroupList = response;
                
                return this.taxGroupList;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);
 }
}

