import { Component, OnInit } from '@angular/core';
import { AppConfiguration } from "../business/appConfiguration";
import { ValidationErrorService } from "../shared/validation-error.service";
import { ErrorDisplayComponent } from '../shared/error-display.component';
import {UserInfo} from "../business/userInfo";
import { DataTablesResponse } from '../entities';
import { HttpClient } from '@angular/common/http';

@Component({
  templateUrl: './customers.component.html'
})
export class CustomersComponent implements OnInit {
  constructor(private config: AppConfiguration, 
              private user:UserInfo,
              private http: HttpClient) {}

  error: ValidationErrorService = new ValidationErrorService();
  dtOptions: DataTables.Settings = {};
  
  ngOnInit(): void {

      const that = this;
      this.config.isSearchAllowed = true;
      this.config.activeTab = "customer";

      var apiUrl = this.config.urls.url("customers");
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
        columns: [{
          title: 'No',
          data: 'no'
        }, {
          title: 'Name',
          data: 'party.name',
          orderable: false
        },
        {
          title:'Address',
          data:'party.address',
          orderable: false
        }, 
        { data: null, orderable: false, render: 
            (data: any, type: any, row: any, meta: any) => {
                var a = `<a href="#/receivable/customers/${data.id}">View</a>`;
                return a;
            } 
        }]
    };
  }
}