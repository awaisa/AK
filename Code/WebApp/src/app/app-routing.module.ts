import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from "./common/login";
import { HomeComponent } from "./home/home.component";

const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: HomeComponent },
    { path: 'login', component: LoginComponent },
    { path: 'payable', loadChildren: 'app/payable/payable.module#PayableModule' },
    { path: 'inventory', loadChildren: 'app/inventory/inventory.module#InventoryModule' },
    { path: 'receivable', loadChildren: 'app/receivable/receivable.module#ReceivableModule' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})
export class AppRoutingModule { }
