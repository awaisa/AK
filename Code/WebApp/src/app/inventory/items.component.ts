import { Component, OnInit } from '@angular/core';
import { AppConfiguration } from "../business/appConfiguration";
import { ErrorInfo } from "../shared/ErrorInfo";
import { ErrorDisplayComponent } from '../shared/error-display.component';
import {UserInfo} from "../business/userInfo";

@Component({
    templateUrl: './items.component.html'
})
export class ItemsComponent implements OnInit {
    constructor(private config: AppConfiguration, private user:UserInfo) {

    }
    
    error: ErrorInfo = new ErrorInfo();
    dtOptions: DataTables.Settings = {};
    
    ngOnInit(): void {

        this.config.searchText = "";
        this.config.isSearchAllowed = true;
        this.config.activeTab = "inventory";

        var t = this.user.token;
        //var apiUrl = this.config.urls.url("item");
        var gridPageSize = this.config.gridPageSize();
          this.dtOptions = {
            ajax: {
                url: this.config.urls.url("items"),
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
              title: 'Code',
              data: 'code'
            }, {
              title: 'Description',
              data: 'description'
            }, {
              title: 'Price',
              data: 'price'
            },
            {
                data: null,
                className: "text-center",
                orderable: false,
                render: function(val, type, row){
                    var a = '<a href="#/inventory/item/'+val.id+'">Edit</a>'
                    return a;
                    //return '<a href="#/inventory/item?id='+val.Code+'" class="btn btn-link">Edit</a> / <a href="" class="editor_remove">Delete</a>';
                }
            }]
          };
    }
}
