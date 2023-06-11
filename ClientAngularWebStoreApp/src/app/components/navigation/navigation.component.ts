import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { Order } from 'src/app/models/order.model';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {
  public isLogged: boolean = false;
  public personId!: string | null ;
  public activate!: string | null ;
  role!: string |  null;

  navTabs: MenuItem[] = [];

  userDashboardOptions: MenuItem[] = [];

  constructor(public orderService: OrderService, private router: Router) { 
    this.checkToken();
  }

  ngOnInit(): void {
    if(localStorage.getItem('token') !== null)
    {
      this.isLogged = true;
      this.role = localStorage.getItem('role');
      this.activate = localStorage.getItem('activate');

      this.navTabs = [
        {
          label: 'Verification',
          icon: 'pi pi-fw pi-check-circle',
          routerLink: ['/home/verification'],
          title: 'Verification',
          visible: this.isLogged && this.role === 'Admin'
        },
        {
          label: 'My orders',
          icon: 'pi pi-fw pi-list',
          routerLink: ['/home/order/order-list'],
          title: 'My orders',
          visible: this.isLogged && this.role === 'Customer'
        },
        {
          label: 'Add product',
          icon: 'pi pi-fw pi-plus-circle',
          routerLink: ['/home/products/new-product'],
          title: 'Add product',
          visible: this.isLogged && this.role === 'Seller' && this.activate === 'Active'
        },
        {
          label: 'All products',
          icon: 'pi pi-fw pi-shopping-bag',
          routerLink: ['/home/products/list-product'],
          title: 'All products',
          visible: this.isLogged && (this.role !== 'Seller' || (this.role === 'Seller' && this.activate === 'Active'))
        },
        {
          label: 'Cart',
          icon: 'pi pi-fw pi-shopping-cart',
          routerLink: ['/home/order'],
          title: 'Cart',
          visible: this.isLogged && this.role === 'Customer'
        }
      ];

      this.userDashboardOptions = [
        {
          label: 'My Profile',
          icon: 'pi pi-fw pi-user-edit',
          routerLink: ['/home/persons', this.personId],
          title: 'My Profile',
        },
        {
          label: 'Logout',
          icon: 'pi pi-fw pi-sign-out',
          routerLink: ['/'],
          command: () => this.logOut(),
          title: 'Logout',
        },
      ];

    }
  }

  logOut() {
    if(localStorage.getItem('order'))
    {
      const item = localStorage.getItem('order');
      const order : Order = JSON.parse(item!);
      this.orderService.deleteOrder(Number(order.id)).subscribe((ifDeleted : boolean) => {
        if (ifDeleted)
        {
          localStorage.removeItem('order');         
          this.router.navigateByUrl(''); 
        }
      });
    }
    localStorage.removeItem('token');
    localStorage.removeItem('personId');
    localStorage.removeItem('role');
    localStorage.removeItem('activate');
    localStorage.removeItem('products');        
    this.router.navigateByUrl(''); 
  }

  checkToken() {
    let data = localStorage.getItem('personId');
    this.personId = data;
  }

}
