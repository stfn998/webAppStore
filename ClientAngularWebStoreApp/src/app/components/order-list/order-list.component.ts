import { Component, OnInit } from '@angular/core';
import { Order } from 'src/app/models/order.model';
import { OrderService } from 'src/app/services/order.service';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.scss']
})
export class OrderListComponent implements OnInit {

  orders: Order[] = []
  currentOrderId: Number;
  customerId: Number;
  orderPreview?: Order;

  constructor(public orderService : OrderService, public productService: ProductService) { 
    this.currentOrderId = -1;
    this.customerId = -1;
  }

  ngOnInit(): void {

    if(localStorage.getItem('order'))
    {
      const item = localStorage.getItem('order');
      const order = JSON.parse(item!);
      this.currentOrderId = order.id;
    }

    if(localStorage.getItem('personId'))
    {
      this.customerId = Number(localStorage.getItem('personId'));
    }
    this.getOrders();

  }

  cancelOrder(order : Order){
    this.orderService.cancelOrder(order).subscribe( ( ) => {
      this.getOrders();
    });
  }

  getOrders(){
    this.orderService.getOrders(this.customerId, this.currentOrderId).subscribe( (orders : Order[] ) => {
      orders.forEach(order => {
        order.totalPrice = 0; // Reset the total price for each order
        order.orderDetails.forEach(orderDetail => {
          const productID = orderDetail.productId;
          const productQuantity = orderDetail.quantity;
    
          this.productService.getProduct(productID).subscribe(product => {
            const productPrice = product.price;
            const totalPriceForProduct = productPrice * productQuantity;
    
            order.totalPrice += totalPriceForProduct;
          });
        });
      });
      this.orders = orders;
    })
  }

  shouldDisableCancel(order: Order): boolean {
    const orderDate = new Date(order.orderDate);
    const currentTime = new Date();
    const timeDiffInMinutes = Math.floor((currentTime.getTime() - orderDate.getTime()) / (1000 * 60));
    
    return timeDiffInMinutes > 60;
  }

  getRemainingTime(order: Order): string {
    if (!order.canCancel || this.isOrderDelivered(order)) {
      return 'N/A'; // Show "N/A" for orders that cannot be cancelled, delivered, or are cancelled
    }
  
    const currentTime = new Date();
    const deliveryTime = new Date(order.deliveryTime);
  
    const timeDiff = deliveryTime.getTime() - currentTime.getTime();
    const hours = Math.floor(timeDiff / (1000 * 60 * 60));
    const minutes = Math.floor((timeDiff % (1000 * 60 * 60)) / (1000 * 60));
  
    return `${hours}h ${minutes}m`;
  }
  

  isOrderDelivered(order: Order): boolean {
    const currentDateTime = new Date();
    return currentDateTime > new Date(order.deliveryTime);
  }

}
