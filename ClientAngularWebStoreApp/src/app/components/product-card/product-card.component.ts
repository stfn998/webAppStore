import { Component, OnChanges, Input, OnInit, SimpleChanges, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Order } from 'src/app/models/order.model';
import { OrderDetail } from 'src/app/models/orderdetail.model';
import { Product } from 'src/app/models/product.model';
import { OrderService } from 'src/app/services/order.service';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent implements OnInit, OnChanges {
  @Input() product?: Product; // Input property for product data
  @Input() role!: string; // Input property for user role

  @Output() updatedProduct = new EventEmitter();

  public showAddToCart! : boolean;
  public orderId? : string;
  public order? : Order;

  constructor(private orderService: OrderService, private productService: ProductService, private messageService: MessageService, private router: Router) { 

    this.showAddToCart = true;
  }

  ngOnInit(): void {
    this.getOrder();
  }

  getOrder() {
    if(localStorage.getItem('order') !== null)
    {
      const item = localStorage.getItem('order');
      const order = JSON.parse(item!);
      this.order = order;
      this.orderId = order.id;

      if (this.order && this.order.orderDetails) {
        for (const orderDetail of this.order.orderDetails) {
          if (orderDetail.productId === this.product?.id) {
            this.showAddToCart = false;
            break;
          }
        }
      }
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes) {
    }
  }

  addToCart(product : Product){
    if(localStorage.getItem('order') !== null)
    {
      const item = localStorage.getItem('order');
      const order = JSON.parse(item!);
      this.order = order;
      this.orderId = order.id;

      if (this.order && this.order.orderDetails) {
        for (const orderDetail of this.order.orderDetails) {
          if (orderDetail.productId === this.product?.id) {
            this.showAddToCart = false;
            break;
          }
        }
      }
    }
    
    if (this.orderId === undefined)
    {
      this.orderService.newOrder(product.id, Number(localStorage.getItem('personId'))).subscribe((data: OrderDetail) => {
        if (data)
        {
          this.showAddToCart = false;
          this.messageService.add({ severity:"success", summary:"Success", detail:"Product added to cart successfully."});
          this.getOrder();
        }
      });
    }
    else
    {
      this.orderService.addProduct(product.id, this.orderService.order?.id ?? '').subscribe((data: OrderDetail) => {
        if (data)
        {
          this.showAddToCart = false;
          this.messageService.add({ severity:"success", summary:"Success", detail:"Product added to cart successfully."});
          this.getOrder();
        }
      });
    }
  }

  removeFromCart(product : Product){
    this.getOrder();
    if (this.orderId === undefined)
    {
      alert("Can not remove product");
    }
    else
    {
      this.orderService.removeProduct(product.id, this.orderId ?? '').subscribe((data: any) => {
        if (data) {
          const updatedOrder = { ...(this.orderService.order as Order) }; // Cast the order as Order type
          updatedOrder.orderDetails = updatedOrder.orderDetails.filter(
            (detail) => detail.productId !== product.id
          );
          this.orderService.order = updatedOrder; // Assign the updated order back to the order property
          this.messageService.add({ severity:"info", summary:"Success", detail:"Product removed from cart successfully."});
          this.showAddToCart = true;
        }
        else{
          this.messageService.add({ severity:"error", summary:"Error", detail:"Error while deleting a product."});
        }
      });
        
    }
  }

  deleteProduct(id : number){
      this.productService.deleteProduct(id)
      .subscribe((data: boolean) => {
        if (data) {
          this.messageService.add({ severity:"success", summary:"Success", detail:"Product deleted successfully."});
          this.productService.getProductsSeller(Number(localStorage.getItem('personId')));
          setTimeout(this.redirect,2000, this.updatedProduct);
        }
        else{
          this.messageService.add({ severity:"error", summary:"Error", detail:"Error while deleting a product."});
        }
    });
  }
  
  redirect(updatedProduct: any) : void{
    updatedProduct.emit();
  }

}
