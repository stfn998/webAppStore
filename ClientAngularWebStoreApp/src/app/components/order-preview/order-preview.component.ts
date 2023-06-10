import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/models/order.model';
import { OrderDetail } from 'src/app/models/orderdetail.model';
import { Product } from 'src/app/models/product.model';
import { OrderService } from 'src/app/services/order.service';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-order-preview',
  templateUrl: './order-preview.component.html',
  styleUrls: ['./order-preview.component.scss']
})
export class OrderPreviewComponent implements OnInit {
  order?: Order;
  products: Product[];
  orderStatus: string;

  constructor(private orderService: OrderService,
              private route: ActivatedRoute,
              private productService: ProductService) { 
      this.products = [];
      this.orderStatus = "";
  }

  ngOnInit(): void {
    this.loadOrder();
  }

  private loadOrder() {
    const id = this.route.snapshot.paramMap.get('orderId');
    const getOrderSub = this.orderService.getOrder(Number(id)).subscribe(order => {
        order.totalPrice = 0; // Reset the total price for each order
        order.orderDetails.forEach(orderDetail => {
          const productID = orderDetail.productId;
          const productQuantity = orderDetail.quantity;
    
          this.productService.getProduct(productID).subscribe(product => {
            const productPrice = product.price;
            const totalPriceForProduct = productPrice * productQuantity;
    
            order.totalPrice += totalPriceForProduct;
            product.quantity = productQuantity;
            this.products.push(product);
          });
        });      
      this.order = order;
      this.orderStatus = this.getOrderStatus(order);
      console.log(order);
      console.log(this.products);
    });
  }

  isOrderDelivered(): boolean {
    const currentDateTime = new Date();
    return currentDateTime > new Date(this.order!.deliveryTime);
  }

  getOrderStatus(order: Order): string {
    if (!order.canCancel) {
      return 'Cancelled';
    } else if (this.isOrderDelivered()) {
      return 'Delivered';
    } else {
      return 'In Delivery';
    }
  }

}
