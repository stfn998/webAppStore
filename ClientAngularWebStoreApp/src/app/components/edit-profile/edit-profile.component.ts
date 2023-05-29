import { DatePipe } from '@angular/common';
import { Component, OnInit, SecurityContext } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import ValidateForm from 'src/app/helpers/validateform';
import { Person } from 'src/app/models/person.models';
import { PersonService } from 'src/app/services/person.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss']
})
export class EditProfileComponent implements OnInit {

  person?: Person;
  isLogged: boolean = false;
  imageUrlSafe!: SafeUrl;
  image:Blob=new Blob();
  editForm: FormGroup = new FormGroup({
    userName: new FormControl('', Validators.required),
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.email, Validators.required]),
    imageUrl: new FormControl('', Validators.required),
    birth: new FormControl('', [Validators.required, Validators.pattern(/^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[012])\/\d{4}$/)]),
    address: new FormControl('', Validators.required),
    shippingCost: new FormControl('', [Validators.required, Validators.min(0)]),
  });
  role!: string |  null;

  dateOfBirth!: Date

  constructor(private personService: PersonService,
              private route: ActivatedRoute,
              private sanitizer: DomSanitizer,
              private datePipe: DatePipe
              ) { }

  ngOnInit(): void {
    this.loadPerson();
    this.dateOfBirth = new Date(this.person?.birth ?? "");
  }

  onSaveChange(data : any) {
    if (data.valid){
        this.personService
      .updatePerson(localStorage.getItem('personId'), this.editForm.value)
      .subscribe((person: Person) => {
        window.alert('Successfully updated a person!');
        this.loadPerson();
        })}
    else{
      alert("Your form is invalid"); //moze i lepse
      return;
    }
  }

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files;
    if (file && file[0]) {
        const reader = new FileReader();

        reader.onload = (e: ProgressEvent<FileReader>) => {
            if(e.target?.result) {
                this.editForm.patchValue({
                  imageUrl: e.target.result.toString() // Populate the form control with base64 string of the image
                });
            }
        }

        reader.readAsDataURL(file[0]);
    }
}

  private loadPerson() {
    const id = this.route.snapshot.paramMap.get('personId');
    const getPersonSub = this.personService.getPerson(Number(id)).subscribe(result => {
      this.person = result;
      this.isLogged = true;
      this.editForm.controls['userName'].setValue(this.person?.userName);
      this.editForm.controls['firstName'].setValue(this.person?.firstName);
      this.editForm.controls['lastName'].setValue(this.person?.lastName);
      this.editForm.controls['email'].setValue(this.person?.email);
      this.editForm.controls['birth'].setValue(this.datePipe.transform(new Date(this.person.birth), 'dd/MM/yyyy'));
      this.editForm.controls['address'].setValue(this.person?.address);
      this.editForm.controls['imageUrl'].setValue(this.person?.imageUrl);
      this.editForm.controls['shippingCost'].setValue(this.person?.shippingCost);
      this.role = localStorage.getItem('role');
    });
    this.personService.getImage(Number(localStorage.getItem('personId'))).subscribe(
      (response: any)=>
      {
        this.createImageFromBlob(response);
      });
  }

  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
       this.imageUrlSafe = this.sanitizer.bypassSecurityTrustUrl(reader.result as string);
       if (this.person) {
        this.person.imageUrl = this.sanitizer.sanitize(SecurityContext.URL, this.imageUrlSafe) ?? "";
      }
      }, false);

    if (image) {
       reader.readAsDataURL(image);
    }
  }

  get firstName() {
    return this.editForm.get('firstName');
  }

  get lastName() {
    return this.editForm.get('lastName');
  }

  get userName() {
    return this.editForm.get('userName');
  }

  get email() {
    return this.editForm.get('email');
  }

  get birth() {
    return this.editForm.get('birth');
  }

  get address() {
    return this.editForm.get('address');
  }
  get imageUrl() {
    return this.editForm.get('imageUrl');
  }

  get shippingCost() {
    return this.editForm.get('shippingCost');
  }

}
