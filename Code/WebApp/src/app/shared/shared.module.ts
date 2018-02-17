import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ErrorDisplayComponent } from './error-display.component';
import { ErrorInfo } from './ErrorInfo';

@NgModule({ 
    imports: [
        RouterModule, CommonModule
    ],
    declarations: [ErrorDisplayComponent],
    exports: [ErrorDisplayComponent, CommonModule, FormsModule],
    providers: [ErrorInfo]
})
export class SharedModule {
}
