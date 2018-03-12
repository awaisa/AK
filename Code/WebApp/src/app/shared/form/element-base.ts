import { OnInit, Input, ElementRef } from '@angular/core';
import {NgModel, ValidationErrors, AbstractControl, ControlContainer} from '@angular/forms';
import {Observable} from 'rxjs';

import {ValueAccessorBase} from './value-accessor';

interface ValidationResult {[validator: string]: string | boolean; }

export abstract class ElementBase<T> extends ValueAccessorBase<T> {
  @Input() public label: string;
  @Input() public placeholder: string;
  @Input() formControlName: string;
  isreadonly: Boolean = false;
  protected abstract model: NgModel;
  public identifier = `app-${this.elementType}-${identifier++}`;

  constructor(
    protected elementType: string,
    protected controlContainer: ControlContainer,
    private _elementRef: ElementRef
  ) {
    super();
  }

  @Input() set readonly( value: boolean ) {
    this.isreadonly = value;
  }

  shouldShowErrors(): boolean {
    const c = this.controlContainer.control.get(this.formControlName);
    if ( c ) {
      return c.errors != null;
    }
  }

  listOfErrors(): string[] {
    const c = this.controlContainer.control.get(this.formControlName);
    return Object.keys(c.errors)
      .map(field => this.message(c.errors, field));
  }

  message(validator: ValidationResult, key: string): string {
    switch (key) {
      case 'required':
        return 'Please enter a value';
      case 'pattern':
        return 'Value does not match required pattern';
      case 'minlength':
        return 'Value must be N characters';
      case 'maxlength':
        return 'Value must be a maximum of N characters';
    }

    switch (typeof validator[key]) {
      case 'string':
        return <string> validator[key];
      default:
        return Object.keys(validator[key]).map(function(k){return validator[key][k]}).join(" ");
        //return `Validation failed: ${key}`;
    }
  }
}
let identifier = 0;
