import { Injectable } from '@angular/core';
import { HttpClientService } from '../business/http-client.service';
import { AppConfiguration } from '../business/appConfiguration';
import { Observable } from 'rxjs';
import { Invoice } from '../entities/invoice';
import { ErrorInfo } from '../shared/ErrorInfo';
import { Tax } from '../entities/tax';

@Injectable()
export class PayableService {

  constructor(private httpClient: HttpClientService,
    private config: AppConfiguration) {
}
invoiceList: Invoice[] = [];
// invoice: Invoice = null;
taxList:Tax[]=[];
// tax:Tax=null;
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
}

