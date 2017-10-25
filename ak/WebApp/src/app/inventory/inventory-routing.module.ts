import { NgModule } from '@angular/core';
import { EnsureAuthenticated } from '../ensureauthenticated';

import {
    Routes,
    RouterModule
} from '@angular/router';

import { ItemsComponent } from './items.component';
//const routes: Routes = [
//    { path: '', redirectTo: 'items', pathMatch: 'full' },
//    { path: 'items', component: ItemsComponent }
//];

@NgModule({
    imports: [RouterModule.forChild([
        { path: '', redirectTo: 'items', pathMatch: 'full' },
        { path: 'items', component: ItemsComponent, canActivate: [EnsureAuthenticated]}
    ])],
    exports: [RouterModule]
})
export class InventoryRoutingModule { }