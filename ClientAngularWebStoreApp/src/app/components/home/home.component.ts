import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Person } from 'src/app/models/person.models';
import { PersonService } from 'src/app/services/person.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  person?: Person;

  constructor(private router: Router, 
              private personService: PersonService) { }

  ngOnInit(): void {
    this.loadPerson();
  }

  loadPerson() {
    this.personService.getPerson(Number(localStorage.getItem('personId')))
      .subscribe((result) => {
        this.person = result;        
    });
  }

}
