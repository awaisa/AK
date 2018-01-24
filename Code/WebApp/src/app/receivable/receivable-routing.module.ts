import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { QuotationsComponent } from './quotations.component';
import { OrdersComponent } from './orders.component';
import { ReceiptsComponent } from './receipts.component';
import { InvoicesComponent } from './invoices.component';
import { CustomersComponent } from './customers.component';
import { EditCustomerComponent } from './edit-customer/edit-customer.component';

//const routes: Routes = ;

@NgModule({
    imports: [RouterModule.forChild([
        { path: '', redirectTo: 'quotations', pathMatch: 'full' },
        { path: 'quotations', component: QuotationsComponent },
        { path: 'orders', component: OrdersComponent },
        { path: 'receipts', component: ReceiptsComponent },
        { path: 'invoices', component: InvoicesComponent },
        { path: 'customers', component: CustomersComponent },
        { path: 'customers/:id', component: EditCustomerComponent }
    ])],
    exports: [RouterModule]
})
export class ReceivableRoutingModule { }