import { Component, Optional, Inject, Input, ViewChild, Host, SkipSelf, ElementRef } from '@angular/core';
import { NgModel, NG_VALUE_ACCESSOR, ControlContainer, } from '@angular/forms';
import {ElementBase, animations} from '../';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'form-text',
  template: `
    <div class="form-group" [ngClass]="{
      'has-danger': shouldShowErrors() == false,
      'has-success': shouldShowErrors() == true
    }">
      <label *ngIf="label" [attr.for]="identifier" class="col-form-label-sm">{{label}}:</label>
      <input *ngIf="!isreadonly"
        class="form-control form-control-sm"
        [ngClass]="{'is-invalid': (shouldShowErrors()) }"
        [(ngModel)]="value"
        type="text"
        [placeholder]="placeholder"
        [id]="identifier"
      />
      <div
        [@flyInOut]="'in,out'"
        class="invalid-feedback"
        *ngIf="!isreadonly && shouldShowErrors()">
        <p *ngFor="let error of listOfErrors()">{{error}}</p>
      </div>
      <p *ngIf="isreadonly" class="form-control-sm form-control-plaintext">{{value}}</p>
    </div>
  `,
  animations,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: FormTextComponent,
    multi: true,
  }],
})
export class FormTextComponent extends ElementBase<string> {

  @ViewChild(NgModel, {static: false}) model: NgModel;

  constructor(
    @Optional() @Host() @SkipSelf() controlContainer: ControlContainer,
    _elementRef: ElementRef
  ) {
    super('text', controlContainer, _elementRef);
  }
}
