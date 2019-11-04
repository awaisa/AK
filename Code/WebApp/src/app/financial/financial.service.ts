import { Injectable } from '@angular/core';
import {AppConfiguration} from "../business/appConfiguration";
// import { HttpClientService } from "../business/http-client.service";
import { HttpClient } from "@angular/common/http";
import { ValidationErrorService } from "../shared/validation-error.service";
import { Observable } from "rxjs";
import { Customer } from '../entities';

@Injectable()
export class FinancialService {
    constructor(private httpClient: HttpClient,
        private config: AppConfiguration) {
    }

    getJournalEntry(id): Observable<Customer> {
        return this.httpClient.get<Customer>(this.config.urls.url("journalEntries", id))
            .map((response) => {
                return response;
            })
            .catch(new ValidationErrorService().parseObservableResponseError);
    }

    saveJournalEntry(customer): Observable<any> {
        return this.httpClient.post(this.config.urls.url("journalEntrySave"), customer, null)
        .map(response => {
            return response;
        })
        .catch(new ValidationErrorService().parseObservableResponseError);
    }
}

