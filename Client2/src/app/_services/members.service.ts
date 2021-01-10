import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, pipe } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { Message } from '../_models/message';
import { PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeaders } from './paginatedHelper';
const httpOptions = {
  headers : new HttpHeaders({
    Authorization :'Bearer ' + JSON.parse(localStorage.getItem('User') || '{}').token
    })
  }
@Injectable({
  providedIn: 'root'
})
export class MembersService {

  basUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map();
  user: User;
  userParams: UserParams;
  
  // userParams: UserParams;
  constructor(private http : HttpClient,private accountservice: AccountService) { 
    this.accountservice.currentUser$.pipe(take(1)).subscribe(user =>{
      this.user = user;
      this.userParams = new UserParams(user);
    })
  }
  getUserParams(){
      return this.userParams;
  }
  setUserParams(params: UserParams){
      this.userParams = params;
  }

  resetUserParams(){
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  getMembers(userParams: UserParams) {

    var response = this.memberCache.get(Object.values(userParams).join('-'));
    if(response)
    {
      return of(response);
    }
    let params = getPaginationHeaders(userParams.pageNumber,userParams.pageSize);
    params = params.append('minAge',userParams.minAge.toString());
    params = params.append('maxAge',userParams.maxAge.toString());
    params = params.append('gender',userParams.gender);
    params = params.append('orderBy',userParams.orderBy);
    
    return getPaginatedResult<Member[]>(this.basUrl +'users',params,this.http)
    .pipe(map(response =>{
      this.memberCache.set(Object.values(userParams).join('-'),response);
      return response;
    }))
  }

  
  // getMembers() {
  //   if(this.members.length > 0) return of(this.members);
  //   return this.http.get<Member[]>(this.basUrl + 'Users').pipe(
  //     map(member => {
  //       this.members = member;
  //       return member;
  //     })
  //   );
  // }

  getMember( username: string ){
    
    const member = [...this.memberCache.values()]
    .reduce((arr,elem) => arr.concat(elem.result),[])
    .find((member: Member) => member.userName === username);
    if(member){
      return of(member);
    }
    return this.http.get<Member>(this.basUrl + '/Users/' + username);
    // const member = this.members.find(x => x.userName === username);
    // if(member !== undefined) return of(member);
    // return this.http.get<Member>(this.basUrl + 'Users/'+ username);
  }

  updateMember(member : Member){
    return this.http.put(this.basUrl + '/users',member).pipe(
      map(() =>{
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
  
  setMainPhoto(photoId: number){
    return this.http.put(this.basUrl + '/users/set-main-photo/'+photoId,{});
  }

  deletePhoto(photoId : number)
  {
    return this.http.delete(this.basUrl + 'users/delete-photo/'+ photoId);
  }

  addLike(username: string){
    return this.http.post(this.basUrl + 'likes/' + username,{});
  }

  getLike(predicate: string, pageNumber, pageSize) {

    let parms = getPaginationHeaders(pageNumber, pageSize);
    parms = parms.append('predicate', predicate);
    return getPaginatedResult<Partial<Member[]>>(this.basUrl + 'likes', parms,this.http);
    // return this.http.get<Partial<Member[]>>(this.basUrl + 'likes?predicate='+ predicate);
  }

  


  

  
}
 