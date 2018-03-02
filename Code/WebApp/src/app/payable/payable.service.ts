import { Injectable } from '@angular/core';
import { HttpClientService } from '../business/http-client.service';
import { AppConfiguration } from '../business/appConfiguration';
import { Observable } from 'rxjs';
import { Invoice } from '../entities/invoice';
import { ErrorInfo } from '../shared/ErrorInfo';
import { Tax } from '../entities/tax';
import { TaxGroup } from '../entities/taxGroup';
import { TaxGroupTax } from '../entities/taxGroupTax';

@Injectable()
export class PayableService {

  constructor(private httpClient: HttpClientService,
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

        return this.httpClient.get(this.config.urls.url("purchaseInvoices"))
            .map(response => {
                this.invoiceList = response.json();
                
                return this.invoiceList;
            })
            .catch(new ErrorInfo().parseObservableResponseError); 
 }

 getInvoice(id) {
  return this.httpClient.get(this.config.urls.url("purchaseInvoices", id))
      .map(response => {
          var result = response.json();
          return this.invoiceList;
      })
      .catch(new ErrorInfo().parseObservableResponseError);
}
getTaxes(force: boolean = false): Observable<Tax[]> {

    // use locally cached version
    if (force !== true && (this.taxList && this.taxList.length > 0))
        return Observable.of(this.taxList) as Observable<Tax[]>;

        return this.httpClient.get(this.config.urls.url("Taxes"))
            .map(response => {
                this.taxList = response.json();
                
                return this.taxList;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
 }
 getTaxGroupTax(force: boolean = false): Observable<TaxGroupTax[]> {

    // use locally cached version
    if (force !== true && (this.taxGroupTaxList && this.taxGroupTaxList.length > 0))
        return Observable.of(this.taxGroupTaxList) as Observable<TaxGroupTax[]>;

        return this.httpClient.get(this.config.urls.url("Taxgrouptax"))
            .map(response => {
                this.taxGroupTaxList = response.json();
                
                return this.taxGroupTaxList;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
 }

 getTaxGroup(force: boolean = false): Observable<TaxGroup[]> {

    // use locally cached version
    if (force !== true && (this.taxGroupList && this.taxGroupList.length > 0))
        return Observable.of(this.taxGroupList) as Observable<TaxGroup[]>;

        return this.httpClient.get(this.config.urls.url("TaxGroup"))
            .map(response => {
                this.taxGroupList = response.json();
                
                return this.taxGroupList;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
 }
}

