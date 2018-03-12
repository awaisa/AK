import { Injectable } from '@angular/core';


import {RequestOptions} from '@angular/http';
declare var toastr: any;
declare var location: any;

@Injectable()
export class AppConfiguration {

      // top level search text
      searchText = '';
      activeTab = 'about';
      isSearchAllowed = true;

      urls = {
        baseUrl: './',
        items: 'api/Inventory',
        brands: 'api/Reference/GetBrand',
        catagories: 'api/Reference/GetCatagory',
        models: 'api/Reference/GetModel',
        taxgroups: 'api/Reference/GetTaxGroup',
        measuremets: 'api/Reference/GetMeasuremets',
        accounts: 'api/Reference/GetAccounts',
        vendorss: 'api/Reference/GetVendors',
        AllItems: "api/Reference/GetItems",
        Taxes: 'api/Reference/GetTaxes',
        Taxgrouptax:"api/Reference/GetTaxGroupTax",
        TaxGroup:"api/Reference/GetTaxGroup",
        paymentTerm:"api/Reference/GetPaymentTerms",

        customers: 'api/Customer',
        invoices: 'api/Sale',
        purchaseInvoices: 'api/Purchase',
        vendors: 'api/Vendor',
        login: 'api/login',
        logout: 'api/logout',
        isAuthenticated: 'api/isAuthenticated',
        reloadData: 'api/reloadData',
        financialAccount: 'api/FinancialAccount',
        url: (name,parm1?,parm2?,parm3?) => {
          let url = this.urls.baseUrl + this.urls[name];
          if (parm1)
            url += '/' + parm1;
          if (parm2)
            url += '/' + parm2;
          if (parm3)
            url += '/' + parm3;

          return url;
        }
      };
     
      constructor() {
          this.setToastrOptions();
          // console.log("AppConfiguration ctor");

          // if(location.port && (location.port == "3999"))
          // this.urls.baseUrl = "http://localhost:5000/"; // kestrel

          this.urls.baseUrl = 'http://localhost:26448/'; // iis Express
          // this.urls.baseUrl = "http://localhost/ak/"; // iis
          // this.urls.baseUrl = "http://vmcore.westus.cloudapp.azure.com/";  // online
      }


      setToastrOptions() {
        toastr.options.closeButton = true;
        toastr.options.positionClass = 'toast-bottom-right';
      }

      gridPageSize(){
        return 10;
      }

  /**
   * Http Request options to for requests
   * @type {RequestOptions}
   */
  requestOptions =  new RequestOptions({  withCredentials: true });
}

