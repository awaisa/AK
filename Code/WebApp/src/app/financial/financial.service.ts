import { Injectable } from '@angular/core';
import {AppConfiguration} from "../business/appConfiguration";
import { HttpClientService } from "../business/http-client.service";
import { ValidationErrorService } from "../shared/validation-error.service";
import { Observable } from "rxjs";
import { Customer } from '../entities';

@Injectable()
export class FinancialService {
    constructor(private httpClient: HttpClientService,
        private config: AppConfiguration) {
    }

    getJournalEntry(id): Observable<Customer> {
        return this.httpClient.get(this.config.urls.url("journalEntries", id))
            .map(response => {
                var result = <Customer>response.json();
                return result;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);
    }

    saveJournalEntry(customer): Observable<any> {
        return this.httpClient.post(this.config.urls.url("journalEntrySave"), customer, null)
        .map(response => {
            return response.json();
        })
        .catch(new ValidationErrorService().parseObservableResponseError);
    }
}

