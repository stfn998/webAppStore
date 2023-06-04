import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Order } from '../models/order.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private orders?: Order[];

  public order?: Order;

  constructor(private http: HttpClient, router: Router) { 
  }

  newOrder(productId: number, customerId: number) : Observable<number> {
    const data = {
      productId: productId,
      customerId: customerId
    };
    return this.http.post<number>(environment.orderServiceUrl + '/api/order', data);
  }

  addProduct(productId: number, orderId: string) : Observable<number> {
    const data = {
      productId: productId,
      orderId: orderId
    };
    return this.http.put<number>(environment.orderServiceUrl + '/api/order', data);
  }

  removeProduct(productId: number, orderId: string) : any {
    const data = {
      productId: productId,
      orderId: orderId
    };
    return this.http.delete<boolean>(environment.orderServiceUrl + '/api/order', { body: data });
  }
  

}
