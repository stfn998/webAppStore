import { Component, Input, OnInit } from '@angular/core';
import { Order } from 'src/app/models/order.model';
import { Product } from 'src/app/models/product.model';
import { OrderService } from 'src/app/services/order.service';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent implements OnInit {
  @Input() product!: Product; // Input property for product data
  @Input() role!: string; // Input property for user role

  public showAddToCart : boolean = true;

  constructor(private orderService: OrderService, private productService: ProductService) { }

  ngOnInit(): void {
  }

  addToCart(product : Product){
    if (this.orderService.order === undefined)
    {
      console.log("prvi if");
      this.orderService.newOrder(product.id, Number(localStorage.getItem('personId'))).subscribe((data: number) => {
        if (data)
        {
          console.log(data);//kako azurirati korpu da pise +1
          this.orderService.order = new Order;
          this.orderService.order.id = data.toString();
          this.showAddToCart = false;
        }
      });
    }
    else
    {
      console.log("drugi if");
      this.orderService.addProduct(product.id, this.orderService.order?.id ?? '').subscribe((data: number) => {
        if (data)
        {
          console.log(data);//kako azurirati korpu da pise +1
          this.showAddToCart = false;
        }
      });
    }
  }

  removeFromCart(product : Product){
    if (this.orderService.order === undefined)
    {
      alert("Can not remove product") //moze i lepse
    }
    else
    {
      this.orderService.removeProduct(product.id, this.orderService.order?.id ?? '').subscribe((data: any) => {
        if (data)
        {
          alert("Product removed from cart") //moze i lepse
          this.showAddToCart = true;
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
