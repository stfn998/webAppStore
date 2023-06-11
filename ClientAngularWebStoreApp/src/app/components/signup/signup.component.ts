import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import ValidateForm from 'src/app/helpers/validateform';
import { PersonService } from 'src/app/services/person.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
})
export class SignupComponent implements OnInit {

  type: string = "password";
  type2: string = "password";
  isText: boolean = false;
  isText2: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  eyeIcon2: string = "fa-eye-slash";
  signUpForm!: FormGroup;
  canSubmitForm : boolean = true;

  constructor(private fb: FormBuilder,
              private router: Router,
              private personService: PersonService,
              private messageService: MessageService) { 
  }

  ngOnInit(): void {
    this.signUpForm = this.fb.group({
      userName: ['', Validators.required],
      email: ['', [Validators.required,Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      birth: ['', [Validators.required, Validators.pattern(/^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$/)]],
      address: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
      personType: ['', Validators.required],
      imageUrl: [null, Validators.required],
    }, { 
      validator: ValidateForm.confirmedValidator('password', 'confirmPassword')
    })
  }

  hideShowPass1() {
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
  }

  hideShowPass2() {
    this.isText2 = !this.isText2;
    this.isText2 ? (this.eyeIcon2 = 'fa-eye') : (this.eyeIcon2 = 'fa-eye-slash');
    this.isText2 ? (this.type2 = 'text') : (this.type2 = 'password');
  }

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files;
    if (file && file[0]) {
        const reader = new FileReader();

        reader.onload = (e: ProgressEvent<FileReader>) => {
            if(e.target?.result) {
                this.signUpForm.patchValue({
                  imageUrl: e.target.result.toString() // Populate the form control with base64 string of the image
                });
            }
        }

        reader.readAsDataURL(file[0]);
    }
}

  onSignUp(){
    if (this.signUpForm.valid){
      this.canSubmitForm =  false;
      if(this.signUpForm.value.personType === 'seller') {
        this.personService.registerSeller(this.signUpForm.value)
          .subscribe(() => {
            this.signUpForm.reset();
            this.messageService.add({ severity:"success", summary:"Success", detail:"You registered successfully."});
            setTimeout(this.redirect,2000, this.router);
          });
      } else if(this.signUpForm.value.personType === 'customer') {
        this.personService.registerCustomer(this.signUpForm.value)
          .subscribe(() => {
            this.signUpForm.reset();
            this.messageService.add({ severity:"success", summary:"Success", detail:"You registered successfully."});
            setTimeout(this.redirect,2000, this.router);
          });
        }
      } 
      else{
        ValidateForm.validateAllFormFiels(this.signUpForm);
        this.messageService.add({ severity:"error", summary:"Error", detail:"Not all fields are valid."});
      }
  }

  redirect(router : Router) : void{
    router.navigateByUrl('');
  }

  get firstName() {
    return this.signUpForm.get('firstName');
  }

  get lastName() {
    return this.signUpForm.get('lastName');
  }

  get userName() {
    return this.signUpForm.get('userName');
  }

  get email() {
    return this.signUpForm.get('email');
  }

  get birth() {
    return this.signUpForm.get('birth');
  }

  get address() {
    return this.signUpForm.get('address');
  }

  get confirmPassword() {
    return this.signUpForm.get('confirmPassword');
  }

  get password() {
    return this.signUpForm.get('password');
  }

  get personType() {
    return this.signUpForm.get('personType');
  }

  get imageUrl() {
    return this.signUpForm.get('imageUrl');
  }

  

}
