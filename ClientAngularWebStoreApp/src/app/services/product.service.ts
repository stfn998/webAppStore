import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private products?: Observable<Product[]>;

  constructor(private http: HttpClient, router: Router) { 
  }

  public getProducts(): Observable<Product[]>  {
    this.products = this.http
      .get<Product[]>(environment.productServiceUrl + '/api/product');
    return this.products;
  }

  public getProductsSeller(idSeller: number): Observable<Product[]>  {
    this.products = this.http
      .get<Product[]>(environment.productServiceUrl + '/api/product/seller/' + idSeller);
      return this.products;
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
}
