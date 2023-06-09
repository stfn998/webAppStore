import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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

  constructor(public orderService: OrderService, private router: Router) { 
    this.checkToken();
  }

  ngOnInit(): void {
    if(localStorage.getItem('token') !== null)
    {
      this.isLogged = true;
      this.role = localStorage.getItem('role');
      this.activate = localStorage.getItem('activate');
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
