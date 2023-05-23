import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import ValidateForm from 'src/app/helpers/validateform';
import { PersonService } from 'src/app/services/person.service';
import { Router } from '@angular/router';
import { Token } from 'src/app/models/token.models';
import { Person } from 'src/app/models/person.models';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  loginForm!: FormGroup ;
  constructor(private fb: FormBuilder,
              private personService: PersonService,
              private router: Router
              ) { }

  ngOnInit(): void {
    if(localStorage.getItem('token') != null)
      this.router.navigateByUrl('');
      this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    })
  }

  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
  }

  onLogin(){
    if (this.loginForm.valid){
      this.personService.login(this.loginForm.value).subscribe(
        (data: Token) => {
          localStorage.setItem('token', data.token);
          localStorage.setItem('personId', data.idPerson.toString());
          localStorage.setItem('role', data.role);
          localStorage.setItem('activate', data.activate);
          this.router.navigateByUrl('/home');
          alert("You are successfully loged in.") //moze i lepse
        },
        error => {
          alert("Authentication failed.");
        }
      )
    }else{
      ValidateForm.validateAllFormFiels(this.loginForm);
      alert("Your form is invalid")
    }
  }

  get username() {
    return this.loginForm.get('username');
  }
  
  get password() {
    return this.loginForm.get('password');
  }

}
