import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
    baseUrl = environment.apiUrl
    registerMode : boolean =   false;
    users : any;
  constructor(private http : HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle()
  {
    this.registerMode = !this.registerMode;

  }

  getUsers(){
    this.http.get( this.baseUrl + 'Users').subscribe(users => this.users = users);
  }
  cancelRegisterMode(event: boolean){
      this.registerMode = event;
  }

}
