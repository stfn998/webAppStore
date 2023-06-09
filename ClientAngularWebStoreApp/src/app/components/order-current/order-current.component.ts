import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validateform';
import { Order } from 'src/app/models/order.model';
import { OrderDetail } from 'src/app/models/orderdetail.model';
import { Product } from 'src/app/models/product.model';
import { OrderService } from 'src/app/services/order.service';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-order-current',
  templateUrl: './order-current.component.html',
  styleUrls: ['./order-current.component.scss'],
})
export class OrderCurrentComponent implements OnInit {
  public products: Product[] = [];
  public total: Number = 0;
  public order?: Order;

  orderForm!: FormGroup;

  constructor(
    public orderService: OrderService,
    public productService: ProductService,
    private router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute
  ) {
  
    const item = localStorage.getItem('order');
    this.order = JSON.parse(item!);

    if (this.order !== null) {
        this.productService
          .getProductsInOrder(Number(this.order?.id))
          .subscribe((data) => {
            if (data !== undefined) {
              this.products = data;
              this.updateTotalSum();
            }
          });
        this.orderForm = this.fb.group({
          address: ['', Validators.required],
          comment: ['']});
    }

  }

  ngOnInit(): void {
  }

  quantityChange(value: number, product : Product){
    if (value > product.quantity) value = product.quantity;
    if (!value) value = 1;
    const orderDetail : OrderDetail | undefined = this.order?.orderDetails.find(ord => ord.productId === product.id);
    if (orderDetail) orderDetail.quantity = value;
    localStorage.setItem('order', JSON.stringify(this.order));
    this.updateTotalSum();
  }

  updateTotalSum(): void {
    if (!this.order) return;
    let totalSum = 0;
    for (let orderDetail of this.order.orderDetails)
    {
      totalSum += orderDetail.quantity * this.products.find(prod => prod.id === orderDetail.productId)!.price;
    }
    this.order.totalPrice = totalSum;
  }

  submitOrder(){
    if (this.orderForm.valid && this.order){
        this.order.deliveryAddress = this.orderForm.get('address')?.value;
        this.order.comment = this.orderForm.get('comment')?.value
        this.orderService.saveOrder(Number(this.order?.id), this.order)
          .subscribe(() => {
            this.orderForm.reset();
            localStorage.removeItem('order');         
            this.router.navigateByUrl('');
            alert("Your order has been submitted.") //moze i lepse
          });
      }
      else{
        ValidateForm.validateAllFormFiels(this.orderForm);
        alert("Your form is invalid") //moze i lepse
      }
  }
  
}
