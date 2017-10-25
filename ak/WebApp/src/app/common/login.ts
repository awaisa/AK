import { Component, OnInit } from '@angular/core';
import {Router, ActivatedRoute} from "@angular/router";

import {UserInfo} from "../business/userInfo";
import {ErrorInfo} from "../shared/ErrorInfo";
import { AppConfiguration } from "../business/appConfiguration";

declare var toastr:any;

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
    returnUrl: string;

    constructor(private user: UserInfo, private route: ActivatedRoute, private router: Router, private config: AppConfiguration) {
        config.activeTab = "login";
    }

    ngOnInit() {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        
        if (this.route.snapshot.url[0].path == "logout")
            this.logout();
    }

    login() {
      this.user.login(this.username,this.password)
        .subscribe((response) => {            
            this.user.isAuthenticated = true;
            toastr.success("You are logged in.");
            //window.location.hash = "about";
            this.router.navigate([this.returnUrl]);
        },
        (err)=> {
          this.error.error(err);
          this.password="";
          toastr.warning("Login failed: " + err.message);
        });
    }

    logout() {
        if(this.user.logout()){
            toastr.success("Logged out.");
            //window.location.hash = "/";
            this.router.navigate(['login']);            
        }
    }

}
