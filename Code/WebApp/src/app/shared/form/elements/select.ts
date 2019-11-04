import {Component, Optional, Inject, Input, ViewChild, ContentChild, AfterContentInit,
  ElementRef, ContentChildren, QueryList, Output } from '@angular/core';
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
      <ng-select *ngIf="!isreadonly" [items]="items"
        bindLabel="name"
        [placeholder]="placeholder"
        appendTo="body"
        [id]="identifier"
        [(ngModel)]="value"
        [ngClass]="{'is-invalid': (shouldShowErrors())}">
          <ng-option [value]="item[bindValue]" [disabled]="item.disabled" *ngFor="let item of items">
            {{getLabel(item)}}
          </ng-option>
      </ng-select>

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
  
  @ViewChild(NgModel, {static: false}) model: NgModel;

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
    var c = this.getDescendantProp(item, 'code');
    if (c) label = label + ' - ' + c;

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
