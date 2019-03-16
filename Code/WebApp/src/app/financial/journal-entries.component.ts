import { Component, OnInit } from '@angular/core';
import { ValidationErrorService } from '../shared/validation-error.service';
import { AppConfiguration } from '../business/appConfiguration';
import { UserInfo } from '../business/userInfo';
import { TitleService } from '../shared/title.service';
import { BreadcrumbsService } from 'ng2-breadcrumbs';
import { HttpClient } from '@angular/common/http';
import { Item, DataTablesResponse } from '../entities';

@Component({
  templateUrl: './journal-entries.component.html'
})
export class JournalEntriesComponent implements OnInit {
  constructor(private config: AppConfiguration, 
    private user:UserInfo, 
    private breadcrumbs:BreadcrumbsService,
    private titleService: TitleService,
    private http: HttpClient) {}

    error: ValidationErrorService = new ValidationErrorService();
    dtOptions: DataTables.Settings = {};

    ngOnInit(): void {
        const that = this;
        this.config.isSearchAllowed = true;
        this.config.activeTab = "financial";

        var gridPageSize = this.config.gridPageSize();
        this.dtOptions = {
            processing: true,
            serverSide: true,
            ordering: true,
            paging: true,
            pageLength:gridPageSize,
            ajax: (dataTablesParameters: any, callback) => {
            that.http
                .post<DataTablesResponse>(
                    this.config.urls.url("items"),
                    dataTablesParameters, {}
                    ).subscribe(resp => {
                    callback({
                        recordsTotal: resp.recordsTotal,
                        recordsFiltered: resp.recordsFiltered,
                        draw: resp.draw,
                        data: resp.data
                    });
                });
            },
            columns: [{ data: 'code', title: 'Code' }, { data: 'description', title: 'Description' }, { data: 'price', title: 'Price' }, { data: null, orderable: false, render: 
                (data: any, type: any, row: any, meta: any) => {
                    var a = `<a href="#/inventory/item/${data.id}">View</a>`;
                    return a;
                } 
            }]
        };
        
        setTimeout(()=>{this.titleService.setTitle("Journal Entries");},0);
    }   
}
