import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ErrorDisplayComponent } from './error-display.component';
import { ValidationErrorService } from './validation-error.service';
import { RefService } from './ref.service';
import { TitleService } from './title.service';
import { NgSelectModule } from '@ng-select/ng-select';

import * as components from './form/elements';
const allComponents = Object.keys(components).map(k => components[k]);

@NgModule({
    imports: [
        RouterModule, CommonModule, FormsModule,
        ReactiveFormsModule, NgSelectModule
    ],
    declarations: [ErrorDisplayComponent, ...allComponents, ],
    exports: [ErrorDisplayComponent, CommonModule, FormsModule, ...allComponents],
    providers: [ValidationErrorService, RefService, TitleService]
})
export class SharedModule {
}
