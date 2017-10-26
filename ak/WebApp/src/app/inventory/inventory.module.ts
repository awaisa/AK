import { NgModule }               from '@angular/core';
import { SharedModule }           from '../shared/shared.module';

import { ItemsComponent }         from './items.component';
import { InventoryRoutingModule } from './inventory-routing.module';

import { InventoryService }       from './inventory.service';
import { EnsureAuthenticated } from '../ensureauthenticated';

import { DataTablesModule } from 'angular-datatables';

@NgModule({
    imports:      [ SharedModule, InventoryRoutingModule, DataTablesModule ],
    declarations: [ ItemsComponent ],
    providers:    [ InventoryService, EnsureAuthenticated ]
})
export class InventoryModule { }