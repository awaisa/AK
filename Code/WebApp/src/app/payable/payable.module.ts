import { NgModule } from '@angular/core';

import { PurchaseOrdersComponent } from './purchase-wrapper/purchase-orders.component';
import { PurchaseInvoicesComponent } from './purchase-invoices.component';
import { VendorsComponent } from './vendors.component';
import { PayableRoutingModule } from './payable-routing.module';

import { DataTablesModule } from 'angular-datatables';
import { SharedModule } from '../shared/shared.module';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [
      PayableRoutingModule,SharedModule,
      DataTablesModule 
  ],
  declarations: [PurchaseOrdersComponent, PurchaseInvoicesComponent, VendorsComponent]
})
export class PayableModule { }
