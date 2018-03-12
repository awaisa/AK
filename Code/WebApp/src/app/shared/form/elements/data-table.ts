import {Component, Input, OnInit } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { slideInLeft, slideIn } from "../../../common/animations";

  @Component({
    // tslint:disable-next-line:component-selector
    selector: 'data-table',
    template: `
                <div class="table-responsive">
                    <table datatable [dtOptions]="dtOptions" class="table table-hover table-sm"></table>
                </div>
    `,
    animations: [slideIn],
    providers: [{
      provide: NG_VALUE_ACCESSOR,
      useExisting: DataTableComponent,
      multi: true,
    }],
  })
  export class DataTableComponent implements OnInit {
    @Input()
    dtOptions: DataTables.Settings = {};
    constructor(){}

    ngOnInit() {
    }
  }
  