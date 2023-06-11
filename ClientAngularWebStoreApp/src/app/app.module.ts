import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { DropdownModule } from 'primeng/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './components/home/home.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { AuthInterceptor } from './services/auth.interceptor';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { EditProfileComponent } from './components/edit-profile/edit-profile.component';
import { DatePipe } from '@angular/common';
import { VerificationComponent } from './components/verification/verification.component';
import { ProductNewComponent } from './components/product-new/product-new.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductUpdateComponent } from './components/product-update/product-update.component';
import { OrderCurrentComponent } from './components/order-current/order-current.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { InputNumberModule } from 'primeng/inputnumber';
import { OrderListComponent } from './components/order-list/order-list.component';
import { OrderPreviewComponent } from './components/order-preview/order-preview.component';
import { MenuModule } from 'primeng/menu';
import { MenubarModule } from 'primeng/menubar';
import { AvatarModule } from 'primeng/avatar';
import { SlideMenuModule } from 'primeng/slidemenu';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    HomeComponent,
    NavigationComponent,
    UserProfileComponent,
    EditProfileComponent,
    VerificationComponent,
    ProductNewComponent,
    ProductListComponent,
    ProductUpdateComponent,
    OrderCurrentComponent,
    ProductCardComponent,
    OrderListComponent,
    OrderPreviewComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    DropdownModule,
    BrowserAnimationsModule,
    CalendarModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    InputNumberModule,
    MenuModule,
    MenubarModule,
    AvatarModule,
    SlideMenuModule,
    TableModule,
    InputTextModule,
    DialogModule,
    ToastModule,
    ],
  providers: [
    MessageService,
    DatePipe,
    {
      provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
