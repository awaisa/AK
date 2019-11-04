import { Injectable } from '@angular/core';
import {AppConfiguration} from "../business/appConfiguration";
import { HttpClient } from "@angular/common/http";
//import { HttpClientService } from "../business/http-client.service";
import { ValidationErrorService } from "../shared/validation-error.service";
import { Observable } from "rxjs";
import { Item, Catagory, Account, Brand, TaxGroup, Vendor, Model, Measurement } from '../entities';

@Injectable()
export class InventoryService {
    constructor(private httpClient: HttpClient,
        private config: AppConfiguration) {}

    item: Item = null;
    error: string = "";

    getItem(id): Observable<Item> {
        return this.httpClient.get<Item>(this.config.urls.url("items", id))
            .map(response => {
                var result = response;
                return result;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);
    }

    saveItem(item): Observable<any> {
        return this.httpClient.post<Item>(this.config.urls.url("items"), item, null)
            .map((response) => {
                return response;
                
                // explicitly update the list with the updated data
                //return this.item;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);
    }
}
