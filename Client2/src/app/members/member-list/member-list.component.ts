import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  members: Member[];
  // members$:Observable<Member[]>;
  pagination: Pagination;
  pageNumber = 1;
  pageSize = 5;
  userParams: UserParams;
  user: User;
  genderList = [{value: 'male', display: 'Male'},{value: 'female', display: 'Females'}] 
  constructor(private memberservice: MembersService,private accountService: AccountService) {
      // this.accountService.currentUser$.pipe(take(1)).subscribe(user =>{
      //   this.user = user;
      //   this.userParams = new UserParams(user);
      // })
      this.userParams = this.memberservice.getUserParams();
   }

  ngOnInit(): void {
    // this.loadMembers();
    // this.members$ = this.memberservice.getMembers();
    this.loadMembers();
  }
  // loadMembers(){
  //   this.memberservice.getMembers().subscribe(Members =>{
  //     this.members= Members;
  //    })
  // }

  loadMembers(){
    this.memberservice.setUserParams(this.userParams);
    this.memberservice.getMembers(this.userParams).subscribe(response =>{
      this.members= response.result;
      this.pagination = response.pagination;
     })
  }
  resetFilters() {
    // this.userParams = new UserParams(this.user);
    this.userParams = this.memberservice.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any){
    this.userParams.pageNumber = event.page;
    this.memberservice.setUserParams(this.userParams);
    this.loadMembers();
  }
}
