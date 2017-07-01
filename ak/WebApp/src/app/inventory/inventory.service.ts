import { Injectable } from '@angular/core';
import { Item } from "../entities/item";
import {AppConfiguration} from "../business/appConfiguration";
import {Http, RequestOptions} from "@angular/http";
import { HttpClientService } from "../business/http-client.service";
import { ErrorInfo } from "../shared/ErrorInfo";
import { Observable } from "rxjs";

@Injectable()
export class InventoryService {
    constructor(private http: Http, private httpClient: HttpClientService,
        private config: AppConfiguration) {
        console.log("InventoryService ctor");
    }

    itemList: Item[] = [];
    item: Item = null;
    error: string = "";

    listScrollPos = 0;


    getItems(force: boolean = false): Observable<Item[]> {

        // use locally cached version
        if (force !== true && (this.itemList && this.itemList.length > 0))
            return Observable.of(this.itemList) as Observable<Item[]>;

        return this.http.get(this.config.urls.url("items"))
            .map(response => {
                this.itemList = response.json();

                return this.itemList;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
    }


    getItem(id) {
        return this.http.get(this.config.urls.url("item", id),
            this.config.requestOptions)
            .map(response => {
                var result = response.json();
                this.item = result.Artist;

                if (!this.itemList || this.itemList.length < 1)
                    this.getItems();

                return result;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
    }

}
