import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserGuard implements CanActivate {
  constructor(private router: Router) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      let token=localStorage.getItem('token');
      if(token!=null)
      {
        let role = localStorage.getItem('role'); 
        if(role==="Customer")
        { 
          return true;
        }
        this.router.navigateByUrl('/home');
        return false;
      }
      this.router.navigateByUrl('');
      return false;
  }
}
  