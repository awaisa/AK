import {Injectable} from "@angular/core";
import {Http, Headers, RequestOptions, Response}  from "@angular/http";
import {UserInfo} from "./userInfo";
import {Observable} from "rxjs";

/**
 * Wrapper around the Http provider to allow customizing HTTP requests
 */
@Injectable()
export class HttpClientService {

  constructor(private http:Http, private user:UserInfo) {
  }

  get(url:string, requestOptions?:RequestOptions)  {
    requestOptions = this.ensureOptions(requestOptions);
    return this.http
      .get(url, requestOptions)
      .catch( response => {
         if (response.status == 401)
           this.user.isAuthenticated = false;

         return Observable.throw(response);
      })
  }

  post(url:string, data:any, requestOptions?:RequestOptions) {
    requestOptions = this.ensureOptions(requestOptions)

    return this.http
      .post(url, data, requestOptions)
      .catch( response => {
        if (response.status == 401)
          this.user.isAuthenticated = false;

        return Observable.throw(response);
      });
  }

  put(url:string, data:any, requestOptions?:RequestOptions) {
    //this.ensureOptions(!requestOptions);

    return this.http
      .put(url, data, requestOptions)
      .catch(response => {
        if (response.status == 401)
          this.user.isAuthenticated = false;

        return Observable.throw(response);
      });
  }

  delete(url:string, requestOptions?:RequestOptions) {

    requestOptions = this.ensureOptions (requestOptions);

    return this.http.delete(url,requestOptions)
      .catch(response => {
        if (response.status == 401)
          this.user.isAuthenticated = false;

        return Observable.throw(response);
      });
  }

  ensureOptions(requestOptions?:RequestOptions):RequestOptions {

    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      let headAuthorization = { 'Authorization': 'Bearer ' + currentUser.token };
      let headers = new Headers(headAuthorization);
      //return new RequestOptions({ headers: headers });
      if (!requestOptions)
        requestOptions = new RequestOptions({
          headers: headers
        });
      else
        requestOptions.headers = headers;
    }

    return requestOptions;
  }
}
