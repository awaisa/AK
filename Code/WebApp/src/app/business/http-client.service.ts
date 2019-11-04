import {Injectable} from "@angular/core";
//import {HttpRequest, Headers, RequestOptions, Response}  from "@angular/common/http";
import { HttpClient } from '@angular/common/http';
import {UserInfo} from "./userInfo";
import {Observable} from "rxjs";

/**
 * Wrapper around the Http provider to allow customizing HTTP requests
 */
@Injectable()
export class HttpClientService {

  constructor(private http:HttpClient, private user:UserInfo) {
  }
  

  get(url:string, requestOptions?:any)  {
    requestOptions = this.ensureOptions(requestOptions);
    return this.http
      .get(url, requestOptions)
      .catch( response => {
         if (response.status == 401)
           this.user.isAuthenticated = false;

         return Observable.throw(response);
      })
  }

  post(url:string, data:any, requestOptions?:any) {
    requestOptions = this.ensureOptions(requestOptions)

    return this.http
      .post(url, data, requestOptions)
      .catch( response => {
        if (response.status == 401)
          this.user.isAuthenticated = false;

        return Observable.throw(response);
      });
  }

  put(url:string, data:any, requestOptions?:any) {
    //this.ensureOptions(!requestOptions);

    return this.http
      .put(url, data, requestOptions)
      .catch(response => {
        if (response.status == 401)
          this.user.isAuthenticated = false;

        return Observable.throw(response);
      });
  }

  delete(url:string, requestOptions?:any) {

    requestOptions = this.ensureOptions (requestOptions);

    return this.http.delete(url,requestOptions)
      .catch(response => {
        if (response.status == 401)
          this.user.isAuthenticated = false;

        return Observable.throw(response);
      });
  }

  ensureOptions(requestOptions?:any):any {

    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      let headAuthorization = {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + currentUser.token
      };
      //return new RequestOptions({ headers: headers });
      
      let options = {
          headers: headAuthorization
      }
      return options;
    }
    return null;
  }
}
