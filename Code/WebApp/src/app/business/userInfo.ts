import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import {AppConfiguration} from "./appConfiguration";
import {Observable} from "rxjs";
import { ValidationErrorService } from "../shared/validation-error.service";
import { User } from '../entities/user';

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
      return this._currentUser.username;
  };

  get fullName() {
      return this._currentUser.firstName + ' ' + this._currentUser.lastName;
  };

  get token() {
    return this._currentUser.token;
  };
  constructor(private http: HttpClient,
              private config: AppConfiguration) {
    // initialize isAuthenticate from localstorage
    var isAuthenticated = localStorage.getItem("av_isAuthenticated");
    this._isAuthenticated = !isAuthenticated || isAuthenticated === 'false' ? false : true;
    this._currentUser = JSON.parse(localStorage.getItem('currentUser'));
 }


  login(username, password) {
    return this.http.post<User>(this.config.urls.url("login"), { username: username, password: password })
      .map((user) => {
          // login successful if there's a jwt token in the response
        //let user = response.json();
          if (user && user.token) {
              // store user details and jwt token in local storage to keep user logged in between page refreshes
              localStorage.setItem('currentUser', JSON.stringify(user));
          }
      })
      .catch( (response) => {
         if(response.status === 401)
          this.isAuthenticated = false;

        return  new ValidationErrorService().parseObservableResponseError(response);
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
    var url = this.config.urls.url("isAuthenticated");
    console.log(url);
    return this.http.get<boolean>(url)
      .map( (response) => {
        this.isAuthenticated = response;
        return this.isAuthenticated;
      })
      .catch( (response) => {
        this.isAuthenticated = false;

        // remove user from local storage to log user out
        localStorage.removeItem('currentUser'); 
        return Observable.throw( response );
      });
  }
}
