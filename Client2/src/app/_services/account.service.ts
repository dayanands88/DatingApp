import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BehaviorSubject, ReplaySubject } from 'rxjs';
import { User } from '../_models/user';
import { map } from 'rxjs/operators';
import { PresenceService } from './presence.service';
@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseURL = environment.apiUrl;
  private currentUserSource = new  ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();
  
  constructor(private http : HttpClient,private presence: PresenceService) { }

  login(model:any)
  {
    return this.http.post(this.baseURL + '/Account/login',model).pipe(
      map((response : User) => {
        const user = response;
        if(user){
        //   localStorage.setItem('user',JSON.stringify(user));
        //  this.setCurrentUser(user);
        this.setCurrentUser(user);
        this.presence.createHubConnection(user);
        }
      })
    )
  }

  register(model : any){
    return this.http.post(this.baseURL + '/account/register',model).pipe(
      map((user: User) => {
        if(user){
         this.setCurrentUser(user);
        this.presence.createHubConnection(user);

          // this.currentUserSource.next(user);
        }
        
      })
    )
  }

  setCurrentUser(user:User)
  {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user',JSON.stringify(user));
    this.currentUserSource.next(user);
  }
  
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presence.stopHubConnection();
  }


  getMember( username: string ){
    // return this.http.get<Member[]>(this.basUrl + 'Users/'+ username,httpOptions);

  }  

  getDecodedToken(token){
    return JSON.parse(atob(token.split('.')[1]));

  }
}
