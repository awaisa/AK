import { NgModule } from '@angular/core';
import { EnsureAuthenticated } from '../ensureauthenticated';
import { Routes, RouterModule } from '@angular/router';

import { AccountsComponent } from './accounts.component';
import { AccountComponent } from './account.component';

@NgModule({
    imports: [RouterModule.forChild([
        { path: '', redirectTo: 'accounts', pathMatch: 'full' },
        { path: 'accounts', component: AccountsComponent, canActivate: [EnsureAuthenticated]},
        { path: 'account/:id', component: AccountComponent, canActivate: [EnsureAuthenticated]},
    ])],
    exports: [RouterModule]
})
export class FinancialRoutingModule { }