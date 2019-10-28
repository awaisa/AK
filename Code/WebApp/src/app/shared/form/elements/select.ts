import {Component, Optional, Inject, Input, ViewChild, ContentChild, AfterContentInit,
  ElementRef, ContentChildren, QueryList,Output,EventEmitter } from '@angular/core';
import {NgModel, NG_VALUE_ACCESSOR, ControlContainer, } from '@angular/forms';
import {ElementBase, animations} from '../';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'form-select',
  template: `
    <div class="form-group" [ngClass]="{
      'has-danger': shouldShowErrors() == false,
      'has-success': shouldShowErrors() == true
    }">
      <label *ngIf="label" [attr.for]="identifier" class="col-form-label-sm font-weight-bold" style="margin-bottom:0px;">{{label}}:</label>
      <select *ngIf="!isreadonly"
        class="form-control form-control-sm"
        [ngClass]="{'is-invalid': (shouldShowErrors())}"
        [(ngModel)]="value"
        [id]="identifier">
          <option value="" selected *ngIf="placeholder">{{placeholder}}</option>
          <option *ngFor="let item of items" [value]="item[bindValue]">{{getLabel(item)}}</option>
      </select>
      <div [@flyInOut]="'in,out'"
        class="invalid-feedback"
        *ngIf="!isreadonly && shouldShowErrors()">
        <p *ngFor="let error of listOfErrors()">{{error}}</p>
      </div>
      <p *ngIf="isreadonly" class="form-control-sm form-control-plaintext">{{getText()}}</p>
    </div>
  `,
  animations,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: FormSelectComponent,
    multi: true,
  }],
})
export class FormSelectComponent extends ElementBase<string> {
  // inputs
  @Input() items: any[] = [];
  @Input() bindLabel: string;
  @Input() bindValue: string;
  @Input() bindOnChange: string;

  @ViewChild(NgModel) model: NgModel;

  constructor(
    _elementRef: ElementRef,
    controlContainer: ControlContainer,
  ) {
    super('select', controlContainer, _elementRef);
  }

  getText(): string {
    // tslint:disable-next-line:triple-equals
    return `${this.getLabel(this.items.find(i => i.id == this.value))}`;
  }

  getLabel(item): string {
    let label = "";
    label = this.getDescendantProp(item, this.bindLabel);
    return label;
  }

  private getDescendantProp(obj, desc) {
      var arr = desc.split(".");
      if(obj != null) {
        while(arr.length && (obj = obj[arr.shift()]));
          return obj;
      }
      return "";
  }

}
