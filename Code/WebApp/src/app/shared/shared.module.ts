import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ErrorDisplayComponent } from './error-display.component';
import { ErrorInfo } from './ErrorInfo';

import * as components from './form/elements';
const allComponents = Object.keys(components).map(k => components[k]);

@NgModule({
    imports: [
        RouterModule, CommonModule, FormsModule,
        ReactiveFormsModule
    ],
    declarations: [ErrorDisplayComponent, ...allComponents, ],
    exports: [ErrorDisplayComponent, CommonModule, FormsModule, ...allComponents],
    providers: [ErrorInfo]
})
export class SharedModule {
}
