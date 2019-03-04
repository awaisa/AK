import { Injectable } from '@angular/core';
import {AppConfiguration} from "../business/appConfiguration";
import { HttpClientService } from "../business/http-client.service";
import { ValidationErrorService } from "../shared/validation-error.service";
import { Observable } from "rxjs";
import { Customer, TaxGroup, Account } from '../entities';

@Injectable()
export class ReceivableService {
    constructor(private httpClient: HttpClientService,
        private config: AppConfiguration) {
        // console.log("InventoryService ctor");
    }

    getCustomer(id): Observable<Customer> {
        return this.httpClient.get(this.config.urls.url("customers", id))
            .map(response => {
                var result = <Customer>response.json();
                return result;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);
    }

    saveCustomer(customer): Observable<any> {
        return this.httpClient.post(this.config.urls.url("customers"), customer, null)
        .map(response => {
            return response.json();
        })
        .catch(new ValidationErrorService().parseObservableResponseError);
    }
}

