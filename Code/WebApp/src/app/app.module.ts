import { NgModule, Injectable} from '@angular/core';
import { BrowserModule} from "@angular/platform-browser";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule} from "@angular/forms";
import { HttpModule, Http, ConnectionBackend, Request, RequestOptionsArgs, Response, RequestOptions, XHRBackend, ResponseOptions, BrowserXhr, XSRFStrategy } from "@angular/http";
import { LocationStrategy, HashLocationStrategy } from '@angular/common';

import {BreadcrumbsModule} from 'ng2-breadcrumbs';

import { HttpClientService } from "./business/http-client.service";
import { SharedModule }           from './shared/shared.module';
import { AppRoutingModule } from "./app-routing.module";

import {AppComponent} from "./app.component";
import { AppConfiguration } from './business/appConfiguration';
import { UserInfo } from "./business/userInfo";

// components
import { AppHeader } from './common/appHeader';
import {AppFooter} from "./common/appFooter";
import {LoginComponent} from "./common/login";
import { HomeComponent } from './home/home.component';

@NgModule({
  imports: [
    BreadcrumbsModule,
    SharedModule, BrowserModule,
    FormsModule,
    HttpModule,
    AppRoutingModule,
    BrowserAnimationsModule
  ],

  // components
  declarations: [
    AppComponent,
    AppHeader,
    AppFooter,
    LoginComponent,
    HomeComponent
  ],
  // services, pipes and providers
  providers   : [
      AppConfiguration,
      UserInfo,
      HttpClientService,

      // make sure you use this for Hash Urls rather than HTML 5 routing
      { provide: LocationStrategy, useClass: HashLocationStrategy },

      // {
      //   provide: XHRBackend,
      //   useFactory: (xhr, opts, strategy, user) => {
      //     return new CoreXHRBackend(xhr, opts, strategy, user);
      //   },
      //   deps: [BrowserXhr, ResponseOptions, XSRFStrategy, UserInfo],
      // }
  ],

  bootstrap: [AppComponent]
})
export class AppModule {

}