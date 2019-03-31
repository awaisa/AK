import { NgModule } from '@angular/core';
import { JournalEntriesComponent } from './journal-enteries/journal-entries.component';
import { GeneralLedgersComponent } from './general-ledgers.component';
import { TaxesComponent } from './taxes.component';
import { AccountsComponent } from './accounts.component';
import { AccountComponent } from './account.component';
import { BanksComponent } from './banks.component';
import { BalanceSheetComponent } from './balance-sheet.component';
import { IncomeStatementComponent } from './income-statement.component';
import { TrialBalanceComponent } from './trial-balance.component';
import { FinancialRoutingModule } from './financial-routing.module';
import { SharedModule } from '../shared/shared.module';
import { DataTablesModule } from 'angular-datatables';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { JournalEntryComponent } from './journal-entry/journal-entry.component';
import { FinancialService } from './financial.service';
import { EnsureAuthenticated } from '../ensureauthenticated';

@NgModule({
  imports: [
    SharedModule, DataTablesModule,
    FormsModule, ReactiveFormsModule,
    FinancialRoutingModule
  ],
  declarations: [JournalEntriesComponent, GeneralLedgersComponent,
    TaxesComponent, AccountsComponent, AccountComponent, BanksComponent, 
    BalanceSheetComponent, IncomeStatementComponent, TrialBalanceComponent , JournalEntryComponent],
    providers:  [ EnsureAuthenticated, FinancialService ]
})
export class FinancialModule { }
