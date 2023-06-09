import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { HomeComponent } from './components/home/home.component';
import { LoginGuard } from './guards/login.guard';
import { LoggedInGuard } from './guards/logged-in.guard';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { EditProfileComponent } from './components/edit-profile/edit-profile.component';
import { AdminGuard } from './guards/admin.guard';
import { VerificationComponent } from './components/verification/verification.component';
import { ProductNewComponent } from './components/product-new/product-new.component';
import { SellerGuard } from './guards/seller.guard';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductUpdateComponent } from './components/product-update/product-update.component';
import { OrderCurrentComponent } from './components/order-current/order-current.component';
import { UserGuard } from './guards/user.guard';
import { OrderListComponent } from './components/order-list/order-list.component';

const routes: Routes = [
  { path:'', component: LoginComponent, canActivate: [LoginGuard] },
  { path: 'signup', component: SignupComponent, canActivate: [LoginGuard] },
  { path: 'home', component: HomeComponent, canActivate: [LoggedInGuard],
       children: [ { path: 'persons/:personId', component: UserProfileComponent, canActivate: [LoggedInGuard]}, 
                   { path: 'persons/edit/:personId', component: EditProfileComponent, canActivate: [LoggedInGuard]},
                   { path: 'verification', component: VerificationComponent, canActivate: [AdminGuard]},
                   { path: 'products/new-product', component: ProductNewComponent, canActivate: [SellerGuard]},
                   { path: 'products/edit/:productId', component: ProductUpdateComponent, canActivate: [SellerGuard]},
                   { path: 'products/list-product', component: ProductListComponent, canActivate: [LoggedInGuard]},
                   { path: 'order', component: OrderCurrentComponent, canActivate: [UserGuard]},
                   { path: 'order/order-list', component: OrderListComponent, canActivate: [UserGuard]},]
  },
  { path:'**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
