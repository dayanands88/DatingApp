import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { PresenceService } from './_services/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating app';
  users: any;
  constructor(private accountservice: AccountService,private presence: PresenceService){}
  ngOnInit(){
    // this.getUsers(); private http: HttpClient,
    this.setCurrentUser();
  }
  // getUsers(){
  //   this.http.get("https://localhost:5001/api/Users").subscribe(response=>{
  //     this.users = response;
  //   },error => {
  //     console.log(error);
  //   })
  // }

  setCurrentUser(){
    let user: User = JSON.parse(localStorage.getItem('user'));
    if(user){
      this.accountservice.setCurrentUser(user);
      this.presence.createHubConnection(user);
    }
   
  }
}
