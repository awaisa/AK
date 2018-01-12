import { NgModule } from '@angular/core';

import { SharedModule }           from '../shared/shared.module';

import { CommonModule } from '@angular/common';
import { QuotationsComponent } from './quotations.component';
import { OrdersComponent } from './orders.component';
import { ReceiptsComponent } from './receipts.component';
import { InvoicesComponent } from './invoices.component';
import { CustomersComponent } from './customers.component';

import { ReceivableRoutingModule } from './receivable-routing.module';

import { DataTablesModule } from 'angular-datatables';

@NgModule({
  imports: [
      SharedModule, CommonModule,
      ReceivableRoutingModule, DataTablesModule
  ],
  declarations: [QuotationsComponent, OrdersComponent, ReceiptsComponent, InvoicesComponent, CustomersComponent]
})
export class ReceivableModule { }
