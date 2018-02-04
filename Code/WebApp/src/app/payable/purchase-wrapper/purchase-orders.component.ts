import { Component, OnInit } from '@angular/core';

import { Item } from '../../entities/item';
import { AppConfiguration } from '../../business/appConfiguration';
import { UserInfo } from '../../business/userInfo';
import { ErrorInfo } from '../../shared/ErrorInfo';

declare var $: any;

@Component({
  templateUrl: 'purchase-orders.component.html',
})
export class PurchaseOrdersComponent implements OnInit  {

  error: ErrorInfo = new ErrorInfo();
  itemList: Item[] = [];
  dtOptions: DataTables.Settings = {};

  constructor(
    private config: AppConfiguration,
    private user: UserInfo
  ) {

  }
  ngOnInit(): void {

            this.config.isSearchAllowed = true;
            this.config.activeTab = 'inventory';
        }
}
