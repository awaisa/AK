import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import {UserInfo} from './business/userInfo';

@Injectable()
export class EnsureAuthenticated implements CanActivate {
  constructor(private user: UserInfo, private router: Router) {}
  canActivate(): boolean {
    if (this.user.isAuthenticated) {
      return true;
    }
    else {
      this.router.navigateByUrl('/login');
      return false;
    }
  }
}
