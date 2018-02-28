import { NgModule } from '@angular/core';

import { PurchaseOrdersComponent } from './purchase-wrapper/purchase-orders.component';
import { PurchaseInvoicesComponent } from './purchase-invoices.component';
import { VendorsComponent } from './vendors.component';
import { PayableRoutingModule } from './payable-routing.module';

import { DataTablesModule } from 'angular-datatables';
import { SharedModule } from '../shared/shared.module';
import { CommonModule } from '@angular/common';
import { InvoiceEditComponent } from './purchase-invoice-edit';
import { EnsureAuthenticated } from '../ensureauthenticated';
import { PayableService } from './payable.service';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { InventoryService } from '../inventory/inventory.service';
@NgModule({
  imports: [
      PayableRoutingModule,SharedModule,ReactiveFormsModule,
      DataTablesModule 
  ],
  declarations: [PurchaseOrdersComponent, PurchaseInvoicesComponent, VendorsComponent,InvoiceEditComponent],
  providers:    [ PayableService,InventoryService, EnsureAuthenticated ]
})
export class PayableModule { }
