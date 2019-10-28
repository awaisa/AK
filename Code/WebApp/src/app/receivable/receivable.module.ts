import { NgModule } from '@angular/core';

import { SharedModule } from '../shared/shared.module';

import { CommonModule } from '@angular/common';
import { QuotationsComponent } from './quotations.component';
import { OrdersComponent } from './orders.component';
import { ReceiptsComponent } from './receipts.component';
import { InvoicesComponent } from './invoices.component';
import { CustomersComponent } from './customers.component';

import { ReceivableRoutingModule } from './receivable-routing.module';

import { DataTablesModule } from 'angular-datatables';
import { EditCustomerComponent } from './edit-customer/edit-customer.component'
import { ReactiveFormsModule } from '@angular/forms';

import { InventoryService } from '../inventory/inventory.service';
import { ReceivableService } from './receivable.service';
import { SaleInvoiceComponent } from './sale-invoice.component';

@NgModule({
  imports: [
      SharedModule, CommonModule,
      ReceivableRoutingModule, DataTablesModule,
      ReactiveFormsModule
  ],
  declarations: [QuotationsComponent, OrdersComponent, 
                  ReceiptsComponent, InvoicesComponent, 
                  CustomersComponent,EditCustomerComponent,SaleInvoiceComponent],
  providers:[InventoryService, ReceivableService]
})
export class ReceivableModule { }
