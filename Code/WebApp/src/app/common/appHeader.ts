import { Component, OnInit } from '@angular/core';
import {AppConfiguration} from "../business/appConfiguration";
import { UserInfo } from "../business/userInfo";

@Component({
    //moduleId: module.id,
    selector: 'app-header',
    templateUrl: 'appHeader.html'
})
export class AppHeader implements OnInit {
    constructor(private config: AppConfiguration, private user: UserInfo) {
    }   

    ngOnInit() {
    }


}
