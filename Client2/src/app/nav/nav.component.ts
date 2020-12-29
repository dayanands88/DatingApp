import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model : any ={} ;
  loggedIn : boolean;

  constructor(public accountservices : AccountService, private router:Router,private toastr : ToastrService) { }

  ngOnInit(): void {

  }
  Login(){
    this.accountservices.login(this.model).subscribe(response => {
      // console.log(response);
      // this.loggedIn = true
        this.router.navigateByUrl('/Members');
    },error =>{
      console.log(error);
        this.toastr.error(error.error);
    }
    
    );
  }
  LogOut(){
    this.loggedIn=false;
    this.accountservices.logout();
    this.router.navigateByUrl('/');
  }

  

}
