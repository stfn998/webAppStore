import { Component, OnChanges, Input, OnInit, SimpleChanges } from '@angular/core';
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

  public showAddToCart! : boolean;
  public orderId? : string;
  public order? : Order;

  constructor(private orderService: OrderService, private productService: ProductService) { 

    this.showAddToCart = true;

    console.log(this.product);

    console.log(this.product?.id);
  }

  ngOnInit(): void {
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
      // Perform any necessary operations when the input property changes
      console.log('Product changed:', this.product);
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
      console.log("prvi if");
      this.orderService.newOrder(product.id, Number(localStorage.getItem('personId'))).subscribe((data: OrderDetail) => {
        if (data)
        {
          console.log(this.orderService.order); //azuriraj korpu da pise +1
          this.showAddToCart = false;
        }
      });
    }
    else
    {
      console.log("drugi if");
      this.orderService.addProduct(product.id, this.orderService.order?.id ?? '').subscribe((data: OrderDetail) => {
        if (data)
        {
          this.showAddToCart = false;
          console.log(this.orderService.order); //azuriraj korpu da pise +1
        }
      });
    }
  }

  removeFromCart(product : Product){
    if (this.orderId === undefined)
    {
      alert("Can not remove product") //moze i lepse
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
          alert("Product removed from cart");
          this.showAddToCart = true;
          console.log(this.orderService.order);
        }
        else{
          alert("Error while removing a product") //moze i lepse
        }
      });
        
    }
  }

  deleteProduct(id : number){
      this.productService.deleteProduct(id)
      .subscribe((data: boolean) => {
        if (data) {
          alert("Product is deleted successfully"); //moze i lepse
          this.productService.getProductsSeller(Number(localStorage.getItem('personId')));
        }
        else{
          alert("Error while deleting a product") //moze i lepse
        }
    });
  }

}
