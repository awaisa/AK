import {Component, Input, OnInit } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { slideInLeft, slideIn } from "../../../common/animations";
import { TitleService } from '../../title.service';

  @Component({
    // tslint:disable-next-line:component-selector
    selector: 'panel-container',
    template: `
                <div class="container" [ngClass]="{ 'hidden': !loaded}" [@slideIn]="'in'">
                  <h2>{{getTitle}}</h2>
                  <div class="card">
                      <div class="card-body">
                          <ng-content></ng-content>
                      </div>
                  </div>
                </div>
    `,
    animations: [slideIn],
    providers: [{
      provide: NG_VALUE_ACCESSOR,
      useExisting: PanelContainerComponent,
      multi: true,
    }],
  })
  export class PanelContainerComponent implements OnInit {
    private title: string;
    
    constructor(private titleService: TitleService){}

    get getTitle(): string {
        return this.title;
    }

    ngOnInit() {
      this.updateTitle = this.updateTitle.bind(this);
      this.titleService.changeEmitted$.subscribe(this.updateTitle);
    }
    
    updateTitle(title){
      this.title = title;
    }  
  }
  