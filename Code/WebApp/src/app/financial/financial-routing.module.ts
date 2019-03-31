import { NgModule } from '@angular/core';
import { EnsureAuthenticated } from '../ensureauthenticated';
import { Routes, RouterModule } from '@angular/router';

import { AccountsComponent } from './accounts.component';
import { AccountComponent } from './account.component';
import { JournalEntriesComponent } from './journal-enteries/journal-entries.component';
import { JournalEntryComponent } from './journal-entry/journal-entry.component';

@NgModule({
    imports: [RouterModule.forChild([
        { path: '', redirectTo: 'accounts', pathMatch: 'full' },
        { path: 'accounts', component: AccountsComponent, canActivate: [EnsureAuthenticated]},
        { path: 'account/:id', component: AccountComponent, canActivate: [EnsureAuthenticated]},
        { path: 'journal-entries', component: JournalEntriesComponent, canActivate: [EnsureAuthenticated]},
        { path: 'journal-entry/:id', component: JournalEntryComponent, canActivate: [EnsureAuthenticated]},
    ])],
    exports: [RouterModule]
})
export class FinancialRoutingModule { }