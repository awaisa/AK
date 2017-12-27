import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OptionsComponent } from "./Options/options";
import { LoginComponent } from "./common/login";
import { AboutComponent } from "./options/about";

const routes: Routes = [
    { path: '', redirectTo: 'payable', pathMatch: 'full' },
    { path: 'payable', loadChildren: 'app/payable/payable.module#PayableModule' },
    { path: 'inventory', loadChildren: 'app/inventory/inventory.module#InventoryModule' },
    { path: 'receivable', loadChildren: 'app/receivable/receivable.module#ReceivableModule' },
	{path: 'options', component: OptionsComponent },
	{ path: 'login', component: LoginComponent },
	{ path: 'logout', component: LoginComponent },
	{ path: 'about', component: AboutComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})
export class AppRoutingModule { }
