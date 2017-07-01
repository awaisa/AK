import { NgModule } from '@angular/core';

import { PurchaseOrdersComponent } from './purchase-orders.component';
import { PurchaseInvoicesComponent } from './purchase-invoices.component';
import { VendorsComponent } from './vendors.component';
import { PayableRoutingModule } from './payable-routing.module';

@NgModule({
  imports: [
      PayableRoutingModule
  ],
  declarations: [PurchaseOrdersComponent, PurchaseInvoicesComponent, VendorsComponent]
})
export class PayableModule { }
