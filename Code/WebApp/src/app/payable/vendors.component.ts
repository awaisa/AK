import { Component, OnInit } from '@angular/core';
import { AppConfiguration } from "../business/appConfiguration";
import { ValidationErrorService } from "../shared/validation-error.service";
import { ErrorDisplayComponent } from '../shared/error-display.component';
import {UserInfo} from "../business/userInfo";
import { DataTablesResponse } from '../entities';
import { HttpClient } from '@angular/common/http';

@Component({
  templateUrl: './vendors.component.html'
})
export class VendorsComponent implements OnInit{

  constructor(private config: AppConfiguration, 
              private user:UserInfo,
              private http: HttpClient) {}

  error: ValidationErrorService = new ValidationErrorService();
  dtOptions: DataTables.Settings = {};

  ngOnInit(): void {

      const that = this;
      this.config.isSearchAllowed = true;
      this.config.activeTab = "vendor";

      var apiUrl = this.config.urls.url("vendors");
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
            data: 'party.name'
          },
          {
            title:'Address',
            data:'party.address'
          }, { data: null, orderable: false, render: 
              (data: any, type: any, row: any, meta: any) => {
                  var a = `<a href="#${apiUrl}/${data.id}">View</a>`;
                  return a;
              } 
          }]
      };
  }
}
