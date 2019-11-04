import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './common/login';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: HomeComponent },
    { path: 'login', component: LoginComponent },
    { path: 'payable', loadChildren: () => import('app/payable/payable.module').then(m => m.PayableModule) },
    { path: 'inventory', loadChildren: () => import('app/inventory/inventory.module').then(m => m.InventoryModule) },
    { path: 'receivable', loadChildren: () => import('app/receivable/receivable.module').then(m => m.ReceivableModule) },
    { path: 'financial', loadChildren: () => import('app/financial/financial.module').then(m => m.FinancialModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})
export class AppRoutingModule { }
