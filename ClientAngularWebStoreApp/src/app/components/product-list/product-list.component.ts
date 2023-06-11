import { Component, OnInit, SecurityContext } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Observable } from 'rxjs';
import { Order } from 'src/app/models/order.model';
import { OrderDetail } from 'src/app/models/orderdetail.model';
import { Product } from 'src/app/models/product.model';
import { OrderService } from 'src/app/services/order.service';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
  role?: string |  null;
  products?: Product[];
  products$?: Observable<Product[]>;

  constructor(public productService: ProductService) { 
    this.updateProducts();
  }

  ngOnInit(): void {
  }

  updateProducts() {
    if(localStorage.getItem('token') !== null)
    {
        this.role = localStorage.getItem('role');
          if (localStorage.getItem('role') === "Seller")
          {
            this.products$ =  this.productService
            .getProductsSeller(Number(localStorage.getItem('personId')));
            // this.productService
            //   .getProductsSeller(Number(localStorage.getItem('personId')))
            //   .subscribe((data: Product[]) => {
            //     if (data) {
            //       this.products = productService.products;
            //     }
            //   });
          }
          else
          {
            this.products$ =  this.productService
            .getProducts();
            // this.productService.getProducts().subscribe((data: Product[]) => {
            //   if (data)
            //   {
            //     this.products = productService.products;
            //   }
            // });
          }
    }
}
}