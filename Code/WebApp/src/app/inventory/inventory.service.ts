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
import { Vendor } from '../entities/vendors';

@Injectable()
export class InventoryService {
    constructor(private httpClient: HttpClientService,
        private config: AppConfiguration) {}

    item: Item = null;
    error: string = "";

    getItem(id): Observable<Item> {
        return this.httpClient.get(this.config.urls.url("items", id))
            .map(response => {
                var result = <Item>response.json();
                return result;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
    }

    saveItem(item): Observable<any> {
        return this.httpClient.post(this.config.urls.url("items"), item, null)
            .map(response => {
                this.item = response.json();
                
                // explicitly update the list with the updated data
                return this.item;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
    }
}
