
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
import { vendor } from '../entities';

@Injectable()
export class ReceivableService {
    constructor(private httpClient: HttpClientService,
        private config: AppConfiguration) {
        //console.log("InventoryService ctor");
    }

    itemList: Item[] = [];
    vendor:vendor;
    
    saveVendor(vendor): Observable<any> {
        return this.httpClient.post(this.config.urls.url('vendors'), vendor, null)
            .map(response => {
                this.vendor = response.json();
                
                // explicitly update the list with the updated data
                // this.updateItem(this.item);
                return this.vendor;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
    }
}

