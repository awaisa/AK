import { Component, OnInit } from '@angular/core';
import {UserInfo} from "../business/userInfo";
import {ErrorInfo} from "../shared/ErrorInfo";
import { AppConfiguration } from "../business/appConfiguration";

declare var toastr:any;

import {ActivatedRoute} from "@angular/router";
//declare var toastr:any;

@Component({
    //moduleId: module.id,
    selector: 'login',
    templateUrl: 'login.html'
})
export class LoginComponent implements OnInit {
    username:string = "";
    password:string = "";
    error: ErrorInfo = new ErrorInfo();

    constructor(private user: UserInfo, private route: ActivatedRoute, private config: AppConfiguration) {
        config.activeTab = "login";
    }

    ngOnInit() {

      if (this.route.snapshot.url[0].path == "logout")
        this.logout();
    }

    login() {
      this.user.login(this.username,this.password)
        .subscribe((response) => {
            var res = response.json();
            this.user.fullName = res.Fullname;
            this.user.userName = res.Username;

            this.user.isAuthenticated = true;
            toastr.success("You are logged in.");
            window.location.hash = "about";
        },
        (err)=> {
          this.error.error(err);
          this.password="";
          toastr.warning("Login failed: " + err.message);
        });
    }

    logout() {
        this.user.logout()
          .subscribe((success) => {
            toastr.success("Logged out.");
            window.location.hash = "albums";
          });
    }

}
