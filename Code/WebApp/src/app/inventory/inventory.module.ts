import { NgModule }               from '@angular/core';
import { SharedModule }           from '../shared/shared.module';

import { ItemsComponent }         from './items.component';
import { ItemEditComponent }         from './item-edit.component';
import { InventoryRoutingModule } from './inventory-routing.module';

import { InventoryService }       from './inventory.service';
import { EnsureAuthenticated } from '../ensureauthenticated';

import { DataTablesModule } from 'angular-datatables';

@NgModule({
    imports:      [ SharedModule, InventoryRoutingModule, DataTablesModule ],
    declarations: [ ItemsComponent, ItemEditComponent ],
    providers:    [ InventoryService, EnsureAuthenticated ]
})
export class InventoryModule { }