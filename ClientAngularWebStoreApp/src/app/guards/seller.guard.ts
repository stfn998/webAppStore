import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class SellerGuard implements CanActivate {
    constructor(private router: Router) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        let token = localStorage.getItem('token');
        if(token != null) {
            let role = localStorage.getItem('role');
            let activate = localStorage.getItem('activate');

            if(role === 'Seller' && activate === 'Active') {
                return true;
            }
            this.router.navigateByUrl('/home');
            return false;
        }
        this.router.navigateByUrl('');
        return false;
    }

}