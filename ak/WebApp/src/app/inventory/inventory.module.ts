import { NgModule }               from '@angular/core';
import { SharedModule }           from '../shared/shared.module';

import { ItemsComponent }         from './items.component';
import { InventoryRoutingModule } from './inventory-routing.module';

import { InventoryService }       from './inventory.service';
import { EnsureAuthenticated } from '../ensureauthenticated';

@NgModule({
    imports:      [ SharedModule, InventoryRoutingModule ],
    declarations: [ ItemsComponent ],
    providers:    [ InventoryService, EnsureAuthenticated ]
})
export class InventoryModule { }