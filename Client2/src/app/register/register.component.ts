import { HttpClient } from '@angular/common/http';
import { Route } from '@angular/compiler/src/core';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  // @Input() usersFromHomeComponent : any ;
  @Output() cancelRegister = new EventEmitter();
 
  registerForms: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];
  constructor(private http : HttpClient,private accountservice : AccountService,
    private fb : FormBuilder,private router: Router) { }

  ngOnInit(): void {
    this.intitializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }
  intitializeForm(){
    this.registerForms =  this.fb.group({
      gender: ['male'],
      username: ['',Validators.required],
      knownAs: ['',Validators.required],
      dateOfBirth: ['',Validators.required],
      city: ['',Validators.required],
      country: ['',Validators.required],
      password: ['',[Validators.required,
          Validators.minLength(4),Validators.maxLength(8)]],
      confirmpassword: ['',[Validators.required,this.matchValues('password')]]
    })
  }
  matchValues(matchTo: string) : ValidatorFn{
    return (control: AbstractControl) =>{
      return control?.value === control?.parent?.controls[matchTo].value 
      ? null :{isMatching: true}
    }
  }

  register(){
    // console.log(this.model);
    this.accountservice.register(this.registerForms.value).subscribe(response =>{
      this.router.navigateByUrl('/members');
      // this.cancel();
    },error => {
      // console.log(error);
      this.validationErrors = error;
    })
  //  console.log(this.registerForms.value);
  }

  cancel(){

    this.cancelRegister.emit(false);

  }

}
