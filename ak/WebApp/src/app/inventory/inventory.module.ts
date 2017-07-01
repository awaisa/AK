import { NgModule }               from '@angular/core';
import { SharedModule }           from '../shared/shared.module';

import { ItemsComponent }         from './items.component';
import { InventoryRoutingModule } from './inventory-routing.module';

import { InventoryService }       from './inventory.service';

@NgModule({
    imports:      [ SharedModule, InventoryRoutingModule ],
    declarations: [ ItemsComponent ],
    providers:    [ InventoryService ]
})
export class InventoryModule { }