import { Component, OnInit } from '@angular/core';

import { Item } from '../../entities/item';
import { AppConfiguration } from "../../business/appConfiguration";
import { UserInfo } from "../../business/userInfo";

declare var $: any;

@Component({
  templateUrl: 'purchase-orders.component.html',
})
export class PurchaseOrdersComponent implements OnInit  {


  itemList: Item[] = [];
  dtOptions: DataTables.Settings = {};

  constructor(
    private config: AppConfiguration, 
    private user:UserInfo
  ){

  }
  ngOnInit(): void {
    
            this.config.searchText = "";
            this.config.isSearchAllowed = true;
            this.config.activeTab = "inventory";
    
            setTimeout(() => {
                $("#SearchBox").focus();
            }, 200);
    
            var t = this.user.token;
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
                pageLength:2,
                lengthChange: false,
                columns: [{
                  title: 'Code',
                  data: 'Code'
                }, {
                  title: 'Description',
                  data: 'Description'
                }, {
                  title: 'Price',
                  data: 'Price'
                },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    render: function(val, type, row){
                        var a = '<a href="#/inventory/item/'+val.Id+'">Edit</a>'
                        return a;
                        //return '<a href="#/inventory/item?id='+val.Code+'" class="btn btn-link">Edit</a> / <a href="" class="editor_remove">Delete</a>';
                    }
                }]
              };
    
            //this.getItems();
        }
}
