import {Component, Optional, Inject, Input, ViewChild, ContentChild, AfterContentInit,
  ElementRef, ContentChildren, QueryList } from '@angular/core';
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
      <label *ngIf="label" [attr.for]="identifier" class="col-form-label-sm">{{label}}</label>
      <select *ngIf="!isreadonly"
        class="form-control form-control-sm"
        [ngClass]="{'is-invalid': (shouldShowErrors())}"
        [(ngModel)]="value"
        [id]="identifier">
          <option value="" disabled selected *ngIf="placeholder">{{placeholder}}</option>
          <option *ngFor="let item of items" [value]="item[bindValue]">{{item[bindLabel]}}</option>
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

  @ViewChild(NgModel) model: NgModel;

  constructor(
    private _elementRef: ElementRef,
    controlContainer: ControlContainer,
  ) {
    super('select', controlContainer);
  }

  getText(): string {
    // tslint:disable-next-line:triple-equals
    return `value: ${this.value} | text: ${this.items.find(i => i.id == this.value)[this.bindLabel]}`;
  }
}
