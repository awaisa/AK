import { Injectable } from '@angular/core';
import {Http, RequestOptions} from "@angular/http";
import {AppConfiguration} from "./appConfiguration";
import {Observable} from "rxjs";
import {ErrorInfo} from "../shared/ErrorInfo";

@Injectable()
export class UserInfo {

  isAdmin = false;
  private _userName:string = null;
  private _fullName: string = null;

  sessionStarted = new Date();

  private _isAuthenticated = false;
  set isAuthenticated(val) {
    this._isAuthenticated = val;
    // cache authentication
    localStorage.setItem('av_isAuthenticated', val.toString());
  }
  get isAuthenticated() {
    return this._isAuthenticated;
  };

  set userName(val) {
      this._userName = val;
      localStorage.setItem('av_userName', val.toString());
  };
  get userName() {
      return this._userName;
  };

  set fullName(val) {
      this._fullName = val;
      localStorage.setItem('av_fullName', val.toString());
  };
  get fullName() {
      return this._fullName;
  };

  constructor(private http: Http,
              private config: AppConfiguration) {
    // initialize isAuthenticate from localstorage
    var isAuthenticated = localStorage.getItem("av_isAuthenticated");
    this._isAuthenticated = !isAuthenticated || isAuthenticated === 'false' ? false : true;
    this._userName = localStorage.getItem("av_userName");
    this._fullName = localStorage.getItem("av_fullName");
 }


  login(username, password) {
    return this.http.post(this.config.urls.url("login"), {
        username: username,
        password: password
      }, new RequestOptions({withCredentials:true}))
      .catch( (response) => {
        if(response.status === 401)
          this.isAuthenticated = false;

        return  new ErrorInfo().parseObservableResponseError(response);
      });
  }

  logout() {
    return this.http.get(this.config.urls.url("logout"),
                         new RequestOptions({withCredentials:true}))
      .map(
        (response) => {
          this.isAuthenticated = false;
          return true;
        }
      );
  }

  /**
   * Calls to the server to check authentication and then
   * updates the local isAuthenticated flag
   * @returns {Observable<R>}
   */
  checkAuthentication() {
    var url = this.config.urls.url( "isAuthenticated" );
    console.log(url);
    return this.http.get(url,
                         new RequestOptions({withCredentials: true}))
      .map( (response) => {
        let result = response.json();
        this.isAuthenticated = result;
        return result;
      })
      .catch( (response) => {
        this.isAuthenticated = false;
        return Observable.throw( response );
      });
  }
}
