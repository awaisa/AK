﻿import { NgModule } from '@angular/core';

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
import { AddCustomerComponent } from './add-customer/add-customer.component'; 

import { InventoryService } from '../inventory/inventory.service';
import { ReceivableService } from './receivable.service';

@NgModule({
  imports: [
      SharedModule, CommonModule,
      ReceivableRoutingModule, DataTablesModule,
      ReactiveFormsModule
  ],
  declarations: [QuotationsComponent, OrdersComponent, ReceiptsComponent, InvoicesComponent, CustomersComponent,EditCustomerComponent, AddCustomerComponent],
  providers:[InventoryService, ReceivableService]
})
export class ReceivableModule { }
