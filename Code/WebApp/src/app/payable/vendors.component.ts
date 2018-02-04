import { Component, OnInit } from '@angular/core';
import { AppConfiguration } from "../business/appConfiguration";
import { ErrorInfo } from "../shared/ErrorInfo";
import { ErrorDisplayComponent } from '../shared/error-display.component';
import {UserInfo} from "../business/userInfo";

@Component({
  templateUrl: './vendors.component.html'
})
export class VendorsComponent implements OnInit{

  constructor(private config: AppConfiguration, private user:UserInfo) {}

  error: ErrorInfo = new ErrorInfo();
  dtOptions: DataTables.Settings = {};

  ngOnInit(): void {

      this.config.isSearchAllowed = true;
      this.config.activeTab = "vendor";

      var t = this.user.token;
      var apiUrl = this.config.urls.url("vendors");
      var gridPageSize = this.config.gridPageSize();
      this.dtOptions = {
          ajax: {
              url: this.config.urls.url("vendors"),
              beforeSend: function(xhr, settings) { 
                  xhr.setRequestHeader('Authorization','Bearer ' + t); 
              } 
          }, 
          serverSide: true,
          ordering: true,
          paging: true,
          pageLength:gridPageSize,
          lengthChange: false,
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
          },
          {
              data: null,
              className: "text-center",
              orderable: false,
              render: function(val, type, row){
                  var a = '<a href="#' + apiUrl + val.id + '">Edit</a>'
                  return a;
                  //return '<a href="#/inventory/item?id='+val.Code+'" class="btn btn-link">Edit</a> / <a href="" class="editor_remove">Delete</a>';
              }
          }]
        };
  }
}
