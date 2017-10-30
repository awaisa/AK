import { Injectable } from '@angular/core';
import { Item } from "../entities/item";
import {AppConfiguration} from "../business/appConfiguration";
import { HttpClientService } from "../business/http-client.service";
import { ErrorInfo } from "../shared/ErrorInfo";
import { Observable } from "rxjs";

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


    getItem(id) {
        return this.httpClient.get(this.config.urls.url("item", id))
            .map(response => {
                var result = response.json();
                this.item = result.Artist;

                if (!this.itemList || this.itemList.length < 1)
                    this.getItems();

                return result;
            })
            .catch(new ErrorInfo().parseObservableResponseError);
    }

    saveItem(item): Observable<any> {
        return this.httpClient.post(this.config.urls.url("item"), item, null)
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
        var i = this.itemList.findIndex((a) => (a.Id == item.Id));
        if (i > -1)
            this.itemList[i] = item;
        else {
            this.itemList.push(item);
            this.itemList.sort((a: Item, b: Item) => {
                var aCode = a.Code.toLocaleLowerCase();
                var bCode = b.Code.toLocaleLowerCase();
                if (aCode > bCode)
                    return 1;
                if (aCode < bCode)
                    return -1;
                return 0;
            })
        }

        this.itemList = this.itemList.filter((a) => a.Id != 0);
    }
}
