import { Injectable, SecurityContext } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  public products?: Product[];

  imageUrlSafe!: SafeUrl;
  image:Blob=new Blob();

  constructor(private http: HttpClient, router: Router, private sanitizer: DomSanitizer) { 
  }

  public getProducts() {
    this.http.get<Product[]>(environment.productServiceUrl + '/api/product').subscribe((products) => {
      this.products = products;
      this.loadImagesForProducts();
    });
  }

  public getProductsSeller(idSeller: number) {
    this.http.get<Product[]>(environment.productServiceUrl + '/api/product/seller/' + idSeller).subscribe((products) => {
      this.products = products;
      this.loadImagesForProducts();
    });
  }

  public addProduct(data : any) {
    return this.http
      .post(environment.productServiceUrl + '/api/product', data);
  }

  public getProduct(id: number): Observable<Product> {
    return this.http
      .get<Product>(environment.productServiceUrl + '/api/product/' + id);
  }

  public updateProduct(idProduct: number | null, data: any) {
    return this.http
      .put<any>(environment.productServiceUrl + '/api/product/' + idProduct, data)
  }

  public deleteProduct(productId: number): any {
    return this.http.delete(environment.productServiceUrl + '/api/product/' + productId);
  }
  
  getImage(idProduct: number): Observable<Blob> {
    return this.http
      .get(environment.productServiceUrl + '/api/product/' + idProduct + '/image', { responseType: 'blob' });
  }

  loadImagesForProducts(): void {
    this.products?.forEach((product) => {
      this.getImage(product.id).subscribe((response: any) => {
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
    });
  }
}
