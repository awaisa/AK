import { Component, OnInit } from '@angular/core';
import { AppConfiguration } from '../business/appConfiguration';
import { ValidationErrorService } from '../shared/validation-error.service';
import { ErrorDisplayComponent } from '../shared/error-display.component';
import {UserInfo} from '../business/userInfo';
import { BreadcrumbsService } from 'ng2-breadcrumbs';
import { TitleService } from '../shared/title.service';
import { Item, DataTablesResponse } from '../entities';
import { HttpClient } from '@angular/common/http';

@Component({
    templateUrl: './items.component.html'
})
export class ItemsComponent implements OnInit {
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
        this.config.activeTab = "inventory";

        var apiUrl = this.config.urls.url("items");
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
                    apiUrl,
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
        
        setTimeout(()=>{this.titleService.setTitle("Items");},0);

    }
}
