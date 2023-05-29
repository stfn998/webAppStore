import { Component, OnInit, SecurityContext } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Product } from 'src/app/models/product.model';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
  public products: Product[] = [];
  role!: string |  null;

  imageUrlSafe!: SafeUrl;
  image:Blob=new Blob();

  constructor(private productService: ProductService, private sanitizer: DomSanitizer) { 

    if(localStorage.getItem('token') !== null)
    {
        this.role = localStorage.getItem('role');
        if (localStorage.getItem('role') === "Seller")
        {
            this.productService.getProductsSeller(Number(localStorage.getItem('personId')))
            .subscribe(products => {
              if (products !== undefined) {
                this.products = products;
                this.loadImagesForProducts();
                console.log(products);
              }
          });
        }
        else
        {
          this.productService.getProducts()
          .subscribe(products => {
            if (products !== undefined) {
              this.products = products;
              this.loadImagesForProducts();
            }
          });
        }
  }
}

  loadImagesForProducts(): void {
    for (const product of this.products) {
      this.productService.getImage(product.id)
        .subscribe((response: any) => {
          let reader = new FileReader();
            reader.addEventListener("load", () => {
              this.imageUrlSafe = this.sanitizer.bypassSecurityTrustUrl(reader.result as string);
              if (product) {
                product.imageUrl = this.sanitizer.sanitize(SecurityContext.URL, this.imageUrlSafe) ?? "";
              }
              }, false);

            if (response) {
              reader.readAsDataURL(response);
            }
        });
    }
  }

  ngOnInit(): void {
  }

  addToCart(id : number){

  }

  deleteProduct(id : number){
      this.productService.deleteProduct(id)
      .subscribe((data: boolean) => {
        if (data) {
          alert("Product is deleted successfully"); //moze i lepse
          this.productService.getProductsSeller(Number(localStorage.getItem('personId')))
            .subscribe(products => {
              if (products !== undefined) {
                this.products = products;
                this.loadImagesForProducts();
              }
          });
        }
        else{
          alert("Error while deleting a product") //moze i lepse
        }
    });
  }

}
