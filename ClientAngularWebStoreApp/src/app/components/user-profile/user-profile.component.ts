import { Component, OnInit, SecurityContext } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { SafeUrl, DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Person } from 'src/app/models/person.models';
import { PersonService } from 'src/app/services/person.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  person?: Person;
  isLogged: boolean = false;
  imageUrl!: SafeUrl;
  image:Blob=new Blob();
  role!: string |  null;

  constructor(private personService: PersonService,
              private route: ActivatedRoute,
              private sanitizer: DomSanitizer
              ) { }

  ngOnInit(): void {
    this.loadPerson();
  }

  private loadPerson() {
    const id = this.route.snapshot.paramMap.get('personId');
    const getPersonSub = this.personService.getPerson(Number(id)).subscribe(result => {
      this.person = result;
      this.isLogged = true;
      this.role = localStorage.getItem('role');
    });
    this.personService.getImage(Number(id)).subscribe(
      (response: any)=>
      {
        this.createImageFromBlob(response);
      });
  }

  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
       this.imageUrl = this.sanitizer.bypassSecurityTrustUrl(reader.result as string);
       if (this.person) {
        this.person.imageUrl = this.sanitizer.sanitize(SecurityContext.URL, this.imageUrl) ?? "";
      }
      }, false);

    if (image) {
       reader.readAsDataURL(image);
    }
  }


}
