import { Injectable } from '@angular/core';
import {Http, Headers, Response, RequestOptions} from "@angular/http";
import {AppConfiguration} from "./appConfiguration";
import {Observable} from "rxjs";
import {ErrorInfo} from "../shared/ErrorInfo";

@Injectable()
export class UserInfo {

  isAdmin = false;
  private _currentUser:any = null;
  private _fullName: string = null;

  sessionStarted = new Date();

  private _isAuthenticated = false;
  set isAuthenticated(val) {
    this._isAuthenticated = val;
    // cache authentication
    localStorage.setItem('av_isAuthenticated', val.toString());
    this._currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }
  get isAuthenticated() {
    return this._isAuthenticated;
  };

  get currentUser() {
      return this._currentUser;
  };

  get userName() {
      return this._currentUser.Username;
  };

  get fullName() {
      return this._currentUser.FirstName + ' ' + this._currentUser.LastName;
  };

  get token() {
    return this._currentUser.token;
  };
  constructor(private http: Http,
              private config: AppConfiguration) {
    // initialize isAuthenticate from localstorage
    var isAuthenticated = localStorage.getItem("av_isAuthenticated");
    this._isAuthenticated = !isAuthenticated || isAuthenticated === 'false' ? false : true;
    this._currentUser = JSON.parse(localStorage.getItem('currentUser'));
 }


  login(username, password) {
    return this.http.post(this.config.urls.url("login"), { username: username, password: password})
      .map((response: Response) => {
          // login successful if there's a jwt token in the response
          let user = response.json();
          if (user && user.token) {
              // store user details and jwt token in local storage to keep user logged in between page refreshes
              localStorage.setItem('currentUser', JSON.stringify(user));
          }
      })
      .catch( (response) => {
        if(response.status === 401)
          this.isAuthenticated = false;

        return  new ErrorInfo().parseObservableResponseError(response);
      });
  }

  logout() {

    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.isAuthenticated = false;
    return true;
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

        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        
        return Observable.throw( response );
      });
  }
}
