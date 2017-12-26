import { NgModule } from '@angular/core';

import { PurchaseOrdersComponent } from './purchase-wrapper/purchase-orders.component';
import { PurchaseInvoicesComponent } from './purchase-invoices.component';
import { VendorsComponent } from './vendors.component';
import { PayableRoutingModule } from './payable-routing.module';

import { DataTablesModule } from 'angular-datatables';

@NgModule({
  imports: [
      PayableRoutingModule,
      DataTablesModule 
  ],
  declarations: [PurchaseOrdersComponent, PurchaseInvoicesComponent, VendorsComponent]
})
export class PayableModule { }
