import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {
  public isLogged: boolean = false;
  public personId!: string | null ;
  role!: string |  null;

  constructor() { }

  ngOnInit(): void {
    if(localStorage.getItem('token') !== null)
    {
      this.isLogged = true;
      this.role = localStorage.getItem('role');
    }
  }

  logOut() {
    localStorage.removeItem('token');
    localStorage.removeItem('personId');
    localStorage.removeItem('role');
    localStorage.removeItem('activate');
  }

  checkToken() {
    let data = localStorage.getItem('personId');
    this.personId = data;
  }

}
