import { Component, OnInit } from '@angular/core';
import { AppConfiguration } from "../business/appConfiguration";
import { ValidationErrorService } from "../shared/validation-error.service";
import { ErrorDisplayComponent } from '../shared/error-display.component';
import {UserInfo} from "../business/userInfo";

@Component({
  templateUrl: './purchase-invoices.component.html'
})
export class PurchaseInvoicesComponent {
  constructor(private config: AppConfiguration, private user:UserInfo) {}

  error: ValidationErrorService = new ValidationErrorService();
  dtOptions: DataTables.Settings = {};
  
  ngOnInit(): void {

      //this.config.searchText = "";
      this.config.isSearchAllowed = true;
      this.config.activeTab = "invoice";

      var t = this.user.token;
      var apiUrl = this.config.urls.url("purchaseInvoices");
      var gridPageSize = this.config.gridPageSize();
      this.dtOptions = {
          ajax: {
              url: this.config.urls.url("purchaseInvoices"),
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
            title: 'VendorName',
            data: 'VendorName'
          }, {
            title:'Date',
            data:'Date'
          },{
            title:'Total',
            data:'total'
          },
          {
            title:'<a href="#' + apiUrl +"/"+ 3+ '">Edit</a>'
            // render: function(val, type, row){
            //   var a = '<a href="#' + apiUrl + val.id + '">Edit</a>'
            //   return a;}
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
