import { Component, OnInit, SecurityContext } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Product } from 'src/app/models/product.model';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product-update',
  templateUrl: './product-update.component.html',
  styleUrls: ['./product-update.component.scss']
})
export class ProductUpdateComponent implements OnInit {

  product?: Product;
  imageUrlSafe!: SafeUrl;
  image:Blob=new Blob();
  canSubmitForm : boolean = true;
  
  productForm: FormGroup = new FormGroup({
    description: new FormControl('', Validators.required),
    quantity: new FormControl('', [Validators.required, Validators.min(1)]),
    price: new FormControl('', [Validators.required, Validators.min(1)]),
    name: new FormControl('', Validators.required),
    imageUrl: new FormControl('', Validators.required)
  });

  constructor(private productService: ProductService,               
              private route: ActivatedRoute,
              private sanitizer: DomSanitizer,
              private router: Router,
              private messageService: MessageService) { 
      this.loadProduct();
              }

  ngOnInit(): void {
  }


  onSaveChange(data : any) {
    if (data.valid){
      this.canSubmitForm =  false;
        this.productService
      .updateProduct(Number(this.route.snapshot.paramMap.get('productId')), this.productForm.value)
      .subscribe((product: Product) => {
        this.messageService.add({ severity:"success", summary:"Success", detail:"Product successfully updated."});
        setTimeout(this.redirect,2000, this.router);
      })}
    else{
      this.messageService.add({ severity:"error", summary:"Error", detail:"Not all fields are valid."});
    }
  }

  redirect(router : Router) : void{
    router.navigateByUrl('/home/products/list-product');
  }

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files;
    if (file && file[0]) {
        const reader = new FileReader();

        reader.onload = (e: ProgressEvent<FileReader>) => {
            if(e.target?.result) {
                this.productForm.patchValue({
                  imageUrl: e.target.result.toString() // Populate the form control with base64 string of the image
                });
            }
        }

        reader.readAsDataURL(file[0]);
    }
  }

  private loadProduct() {
    const id = this.route.snapshot.paramMap.get('productId');
    this.productService.getProduct(Number(id)).subscribe(result => {
      this.product = result;
      this.productForm.controls['name'].setValue(this.product?.name);
      this.productForm.controls['description'].setValue(this.product?.description);
      this.productForm.controls['quantity'].setValue(this.product?.quantity);
      this.productForm.controls['price'].setValue(this.product?.price);
      this.productForm.controls['imageUrl'].setValue(this.product?.imageUrl);
    });
    this.productService.getImage(Number(id)).subscribe(
      (response: any)=>
      {
        this.createImageFromBlob(response);
      });
  }

  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
       this.imageUrlSafe = this.sanitizer.bypassSecurityTrustUrl(reader.result as string);
       if (this.product) {
        this.product.imageUrl = this.sanitizer.sanitize(SecurityContext.URL, this.imageUrlSafe) ?? "";
      }
      }, false);

    if (image) {
       reader.readAsDataURL(image);
    }
  }

  get name() {
    return this.productForm.get('name');
  }

  get price() {
    return this.productForm.get('price');
  }

  get description() {
    return this.productForm.get('description');
  }

  get imageUrl() {
    return this.productForm.get('imageUrl');
  }

  get quantity() {
    return this.productForm.get('quantity');
  }

}
