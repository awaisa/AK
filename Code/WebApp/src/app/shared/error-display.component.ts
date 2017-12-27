import {Component, OnInit, Input} from '@angular/core';
import {ResponseOptionsArgs, Response} from "@angular/http";
import {Observable} from "rxjs";

import { ErrorInfo } from '../shared/ErrorInfo';


/**
 *   A Bootstrap based alert display
 */
@Component({
    //moduleId: module.id,
    selector: 'error-display',
    //templateUrl: 'errorDisplay.html'
    template:  `
<div *ngIf="error.message"
     class="alert alert-{{error.icon}} alert-dismissable">
    <button *ngIf="error.dismissable" type="button" class="close"
             data-dismiss="alert" aria-hidden="true">
        <i class="fa fa-remove"></i>
    </button>

    <div *ngIf="error.header" style="font-size: 1.5em; font-weight: bold">
        <i class="fa fa-{{error.imageIcon}}" style="color: {{error.iconColor}}"></i>
        {{error.header}}
    </div>
    <i *ngIf="!error.header"
       class="fa fa-{{error.imageIcon}}"
       style="color: {{error.iconColor}}"></i>

    <strong>{{error.message}}</strong>
</div>
`
})

export class ErrorDisplayComponent implements OnInit {
  constructor() {
  }

  /**
   * Error object that is bound to the component.
   * @type {ErrorInfo}
   */
  @Input() error: ErrorInfo = new ErrorInfo();

  ngOnInit() { }
}
