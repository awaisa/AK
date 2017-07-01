
import {NgModule, Injectable} from '@angular/core'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {FormsModule} from "@angular/forms";
import {BrowserModule} from "@angular/platform-browser";

import { AppRoutingModule } from "./app-routing.module";
import {AppComponent} from "./app.component";

import {
  HttpModule, Http, ConnectionBackend, Request, RequestOptionsArgs, Response,
  RequestOptions, XHRBackend, ResponseOptions, BrowserXhr, XSRFStrategy
} from "@angular/http";
import { LocationStrategy, HashLocationStrategy } from '@angular/common';


// components
import { AppHeader } from './common/appHeader';
import {AppFooter} from "./common/appFooter";


// services
//import { Album, Artist, Track } from './business/entities';

import { AppConfiguration } from './business/appConfiguration';
import { UserInfo } from "./business/userInfo";

// directives and shared components

import { OptionsComponent } from "./Options/options";
import {LoginComponent} from "./common/login";

import { HttpClientService } from "./business/http-client.service";
import {AboutComponent} from "./options/about";


// Enable production mode
// import { enableProdMode } from '@angular/core';
// enableProdMode();

@NgModule({
  imports: [
    BrowserModule,
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

    AboutComponent,

    LoginComponent,
    OptionsComponent

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