import { Component, OnInit, SecurityContext } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
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

  public products?: Product[];

  constructor(public productService: ProductService) { 

    if(localStorage.getItem('token') !== null)
    {
        this.role = localStorage.getItem('role');
        if (localStorage.getItem('role') === "Seller")
          {
            this.productService.getProductsSeller(Number(localStorage.getItem('personId')));
          }
        else
        {
          this.productService.getProducts();
        }
    }
  }

  ngOnInit(): void {
  }

}