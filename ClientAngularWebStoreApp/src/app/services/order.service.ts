import { Injectable } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';
import { Order } from '../models/order.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { OrderDetail } from '../models/orderdetail.model';
import { ProductService } from './product.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  public order!: Order;

  constructor(private http: HttpClient, router: Router, private productService: ProductService) { 
    if(localStorage.getItem('order') !== null)
    {
      const item = localStorage.getItem('order');
      const order = JSON.parse(item!);
      this.order = order;
    }
  }

  newOrder(productId: number, customerId: number) : Observable<OrderDetail> {
    const data = {
      productId: productId,
      customerId: customerId
    };
    return this.http.post<OrderDetail>(environment.orderServiceUrl + '/api/order', data).pipe(
      tap((orderDetail: OrderDetail) => {
        if (orderDetail) {
          this.createOrder(orderDetail);
          this.getOrderSum();
          this.getOrder(Number(this.order.id)).subscribe((data2: Order) => {
          this.setOrderFields(data2);
          localStorage.setItem('order',JSON.stringify(this.order));
          });
        }
      })
    );
  }

  addProduct(productId: number, orderId: string) : Observable<OrderDetail> {
    const data = {
      productId: productId,
      orderId: orderId
    };
    return this.http.put<OrderDetail>(environment.orderServiceUrl + '/api/order', data)
    .pipe(
      tap((response: OrderDetail) => {
        const orderDetail: OrderDetail = {
          customerId: response.customerId,
          productId: response.productId,
          orderId: response.orderId,
          quantity: response.quantity
        };
        this.order?.orderDetails.push(orderDetail);
        this.productService.getProduct(orderDetail.productId).subscribe(product => {
          this.order.totalPrice += product.price;
          this.getOrder(Number(this.order.id)).subscribe((data2: Order) => {
            this.setOrderFields(data2);
            localStorage.setItem('order', JSON.stringify(this.order));
          });
        });
      })
    );
  }

  removeProduct(productId: number, orderId: string) : any {
    const data = {
      productId: productId,
      orderId: orderId
    };
    return this.http.delete<boolean>(environment.orderServiceUrl + '/api/order', { body: data })
    .pipe(
      tap((response: boolean) => {
        if (response){
          const item = localStorage.getItem('order');
          this.order = JSON.parse(item!);
          this.order.orderDetails = this.order.orderDetails.filter(
            (detail) => detail.productId !== productId
          );
          this.productService.getProduct(productId).subscribe(product => {
            this.order.totalPrice -= product.price;
            this.getOrder(Number(this.order.id)).subscribe((data2: Order) => {
              this.setOrderFields(data2);
              localStorage.setItem('order', JSON.stringify(this.order));
            });
          });
        }
      }));
  }
  
  saveOrder(idOrder: number | null, order : Order) : Observable<OrderDetail> {
    console.log(order);
    return this.http.put<OrderDetail>(environment.orderServiceUrl + '/api/order/' + idOrder, order);
  }

  public deleteOrder(orderId: number): Observable<boolean> {
    return this.http.delete<boolean>(environment.orderServiceUrl + '/api/order/' + orderId);
  }

  getOrder(id: number): Observable<Order> {
    return this.http
      .get<Order>(environment.orderServiceUrl + '/api/order/' + id);
  }

  cancelOrder(order : Order) : Observable<boolean> {
    return this.http.put<boolean>(environment.orderServiceUrl + '/api/order/cancel', order);
  }

  public getOrders(idPerson: Number, currentOrderId: Number): Observable<Order[]> {
    const params = {
      currentOrderId: currentOrderId.toString()
    };
    return this.http.get<Order[]>(`${environment.orderServiceUrl}/api/order/${idPerson}/orders`, { params });
  }

  async createOrder(orderDetail: OrderDetail): Promise<void> {
    this.order = {
      id: orderDetail.orderId.toString(),
      customerId: orderDetail.customerId,
      deliveryAddress: '',
      comment: '',
      orderDate: '',
      deliveryTime: '',
      canCancel: true,
      shippingCost: 0,
      totalPrice: 0,
      idOrder: orderDetail.orderId,
      orderDetails: [
        {
          customerId: orderDetail.customerId,
          productId: orderDetail.productId,
          orderId: orderDetail.orderId,
          quantity: orderDetail.quantity
        }
      ]
    };
  }

  getOrderSum(): void{
    const productIDs = this.order.orderDetails.map(od => od.productId);
    productIDs.forEach(pId => { 
      this.productService.getProduct(pId).subscribe(product => {
        this.order.totalPrice += product.price;;
      });
    })
  }

  setOrderFields(order: Order): void {
    this.order.shippingCost = order.shippingCost;
    this.order.deliveryTime = order.deliveryTime;
    this.order.orderDate = order.orderDate;
    this.order.canCancel = order.canCancel;
  }

}
