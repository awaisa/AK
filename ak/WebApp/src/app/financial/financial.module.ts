import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JournalEntriesComponent } from './journal-entries.component';
import { GeneralLedgersComponent } from './general-ledgers.component';
import { TaxesComponent } from './taxes.component';
import { AccountsComponent } from './accounts.component';
import { BanksComponent } from './banks.component';
import { BalanceSheetComponent } from './balance-sheet.component';
import { IncomeStatementComponent } from './income-statement.component';
import { TrialBalanceComponent } from './trial-balance.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [JournalEntriesComponent, GeneralLedgersComponent, TaxesComponent, AccountsComponent, BanksComponent, BalanceSheetComponent, IncomeStatementComponent, TrialBalanceComponent]
})
export class FinancialModule { }
