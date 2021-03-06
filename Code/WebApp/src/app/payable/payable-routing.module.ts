﻿import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PurchaseOrdersComponent } from './purchase-wrapper/purchase-orders.component';
import { PurchaseInvoicesComponent } from './purchase-invoices.component';
import { VendorsComponent } from './vendors.component';
import { EnsureAuthenticated } from '../ensureauthenticated';
import { InvoiceEditComponent } from './purchase-invoice-edit';
//const routes: Routes = [
//    { path: '', redirectTo: 'purchase-orders', pathMatch: 'full' },
//    { path: 'purchase-orders', component: PurchaseOrdersComponent },
//    { path: 'purchase-invoices', component: PurchaseInvoicesComponent },
//    { path: 'vendors', component: VendorsComponent }
//];

@NgModule({
    imports: [RouterModule.forChild([
        { path: '', redirectTo: 'purchase-orders', pathMatch: 'full' },
        { path: 'purchase-orders', component: PurchaseOrdersComponent },
        { path: 'purchase-invoices', component: PurchaseInvoicesComponent },
        { path: 'vendors', component: VendorsComponent },
        { path: 'purchase-invoice-edit/:id', component: InvoiceEditComponent},
    ])],
    exports: [RouterModule]
})
export class PayableRoutingModule { }