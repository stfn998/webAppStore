import { Component, OnInit, SecurityContext } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import ValidateForm from 'src/app/helpers/validateform';
import { Person } from 'src/app/models/person.models';
import { Product } from 'src/app/models/product.model';
import { PersonService } from 'src/app/services/person.service';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product-new',
  templateUrl: './product-new.component.html',
  styleUrls: ['./product-new.component.scss']
})
export class ProductNewComponent implements OnInit {

  product? : Product;
  isLogged: boolean = false;
  imageUrlSafe!: SafeUrl;
  image:Blob=new Blob();
  productForm!: FormGroup;
  role!: string |  null;
  person?: Person;
  canSubmitForm : boolean = true;
  
  constructor(private sanitizer: DomSanitizer, private messageService: MessageService, private fb: FormBuilder, private productService: ProductService, private personService: PersonService, private router: Router) { }

  ngOnInit(): void {
    this.productForm = this.fb.group({
      description: ['', Validators.required],
      quantity: ['',[Validators.required,Validators.min(1)]],
      price: ['',[Validators.required,Validators.min(1)]],
      name: ['', Validators.required],
      imageUrl: [null, Validators.required]
    })
    this.loadPerson();
  }

  onAddProduct(){
    if (this.productForm.valid){
        this.canSubmitForm =  false;
        this.productForm.addControl('sellerId', new FormControl());
        this.productForm.patchValue({"sellerId": this.person?.id});
        this.productService.addProduct(this.productForm.value)
        .subscribe(() => {
          this.messageService.add({ severity:"success", summary:"Success", detail:"Product successfully added"});
          setTimeout(this.redirect,2000, this.router);
        });
      } 
      else{
        ValidateForm.validateAllFormFiels(this.productForm);
        this.messageService.add({ severity:"error", summary:"Error", detail:"Not all fields are valid."});
      }
  }

  redirect(router : Router) : void{
    router.navigateByUrl('/home/products/list-product');
  }

  private loadPerson() {
    const id = localStorage.getItem('personId');
    this.personService.getPerson(Number(id)).subscribe(result => {
      this.person = result;
    });
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
