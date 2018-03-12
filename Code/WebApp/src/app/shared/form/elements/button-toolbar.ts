import {Component, Optional, Inject, Input, ViewChild, ContentChild, AfterContentInit,
    ElementRef, ContentChildren, QueryList, Output, EventEmitter } from '@angular/core';
  import {NgModel, NG_VALUE_ACCESSOR, ControlContainer, } from '@angular/forms';
  import {ElementBase, animations} from '../';
  
  @Component({
    // tslint:disable-next-line:component-selector
    selector: 'button-toolbar',
    template: `
        <hr *ngIf="!isTop" />
        <button (click)="modeChanged()" *ngIf="!isEditMode" type="button" class="btn btn-primary btn-sm">Edit</button>
        <button (click)="modeChanged()" *ngIf="isEditMode && !hideCancel" type="button" class="btn btn-light btn-sm">Cancel</button>
        <button (click)="onSave(isTop)" *ngIf="isEditMode" type="button" class="btn btn-primary btn-sm">Save</button>
        <a href={{backUrl}} class="btn btn-secondary btn-sm">Back</a>
        <hr *ngIf="isTop" />
    `,
    animations,
    providers: [{
      provide: NG_VALUE_ACCESSOR,
      useExisting: ButtonToolbarComponent,
      multi: true,
    }],
  })
  export class ButtonToolbarComponent {
    // inputs
    @Input() backUrl: string;
    @Input() isEditMode: boolean;
    @Input() isTop: boolean = true;
    @Input() hideCancel: boolean = true;
    
    @Output() change: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() save: EventEmitter<any> = new EventEmitter<any>();

    @ViewChild(NgModel) model: NgModel;
  
    constructor(
      private _elementRef: ElementRef,
      controlContainer: ControlContainer,
    ) {}

    modeChanged(){
        this.isEditMode = !this.isEditMode;
        this.change.emit(this.isEditMode);
    }
    onSave(top){
        this.save.emit();
    }
  }
  