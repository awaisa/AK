import { Component, OnInit } from '@angular/core';
import { ErrorInfo } from '../shared/ErrorInfo';
import { AppConfiguration } from '../business/appConfiguration';
import {UserInfo} from '../business/userInfo';

@Component({
  templateUrl: './accounts.component.html'
})
export class AccountsComponent implements OnInit {

  error: ErrorInfo = new ErrorInfo();
  dtOptions: DataTables.Settings = {};

  constructor(private config: AppConfiguration, private user: UserInfo) {

  }

  ngOnInit(): void {

      // this.config.searchText = "";
      this.config.isSearchAllowed = true;
      this.config.activeTab = 'financial';

      const t = this.user.token;
      const gridPageSize = this.config.gridPageSize();
        this.dtOptions = {
          ajax: {
              url: this.config.urls.url('financialAccount'),
              beforeSend: function(xhr, settings) {
                  xhr.setRequestHeader('Authorization', 'Bearer ' + t);
              }
          },
          serverSide: true,
          ordering: true,
          paging: true,
          pageLength: gridPageSize,
          lengthChange: false,
          columns: [{
            title: 'Code',
            data: 'accountCode'
          }, {
            title: 'Name',
            data: 'accountName'
          }, {
            title: 'Class',
            data: 'accountClass'
          },
          {
              data: null,
              className: 'text-center',
              orderable: false,
              render: function(val, type, row){
                  const a = '<a href="#/financial/account/' + val.id + '">Edit</a>';
                  return a;
              }
          }]
        };
  }
}

