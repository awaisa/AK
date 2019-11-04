import { Component, Optional, Inject, Input, ViewChild, Host, SkipSelf, Output, EventEmitter, ElementRef } from '@angular/core';
import { NgModel, NG_VALUE_ACCESSOR, ControlContainer, FormGroup, } from '@angular/forms';
import {ElementBase, animations} from '../';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'form-radio',
  template: `

      <label *ngIf="label" [attr.for]="identifier" class="col-form-label-sm font-weight-bold" style="margin-bottom:0px;">{{label}}:</label>
      <div *ngIf="!isreadonly" class="custom-control custom-radio">
        <input
          type="radio" 
          class="custom-control-input"
          [checked]="value == true"
          [value]="value"
          [name]="formControlName" 
          [id]="identifier" (change)="onSelectionChange(this)">
          <label [attr.for]="identifier" class="custom-control-label">{{placeholder}}</label>
      </div>
      <div
        [@flyInOut]="'in,out'"
        class="invalid-feedback"
        *ngIf="!isreadonly && shouldShowErrors()">
        <p *ngFor="let error of listOfErrors()">{{error}}</p>
      </div>
      <p *ngIf="isreadonly">
        <i *ngIf="value" class="fa fa-check-circle"></i>
        <i *ngIf="!value" class="fa fa-circle"></i>
      </p>
  `,
  animations,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: FormRadioComponent,
    multi: true,
  }],
})
export class FormRadioComponent extends ElementBase<boolean> {

  @ViewChild(NgModel, {static: false}) model: NgModel;
  @Output() radioChange: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    @Optional() @Host() @SkipSelf() controlContainer: ControlContainer, _elementRef: ElementRef
  ) {
    super('radio', controlContainer, _elementRef);
  }
  
  onSelectionChange(entry) {
    this.radioChange.emit();
    let fg = this.controlContainer.control as FormGroup;
    fg.get(this.formControlName).setValue(true);
  }

}
