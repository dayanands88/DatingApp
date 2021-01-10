import { Component, OnInit } from '@angular/core';
import { BsModalService,BsModalRef } from 'ngx-bootstrap/modal';

import { RoleModalComponent } from 'src/app/modals/role-modal/role-modal.component';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: Partial<User[]>;
  bsModalRef: BsModalRef;
  constructor(private adminService: AdminService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUserWithRoles();

  }

  getUserWithRoles(){
      this.adminService.getUsersWithRoles().subscribe(users => {
        this.users = users;
      })
  }

  // OpenRolesModal(user: User){
  //   const config = {
  //     initialState: [
  //      user

  //     ],
  //     title: 'Modal with component'
  //   };
  //   this.bsModalRef = this.modalService.show(RoleModalComponent);
  //   this.bsModalRef.content.closeBtnName = 'Close';
  // }

  OpenRolesModal(user: User) {
    
    const config ={
      class:'modal-dialog-centered',
      initialState: {
        user,
        roles: this.getRolesArray(user)
      }
    }

    this.bsModalRef = this.modalService.show(RoleModalComponent, config);
    this.bsModalRef.content.updateSelectedRoles.subscribe(values =>{
      const roleToUpdate = {
        roles:[...values.filter(el => el.checked === true).map(el => el.name)]
      };
      if(roleToUpdate){
        this.adminService.updateUserRoles(user.userName,roleToUpdate.roles).subscribe(() =>{
          user.roles = [...roleToUpdate.roles]
        })
      }
    });
  }


  private getRolesArray(user) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] =[
      { name: 'Admin', value:'Admin' },
      { name: 'Moderator', value:'Moderator' },
      { name: 'Member', value:'Member' }
    ];
    availableRoles.forEach(role =>{
      let isMatch = false;
      for(const userRole of userRoles){
        if(role.name === userRole) {
          isMatch = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }
      if(!isMatch){
        role.checked = false;
        roles.push(role);
      }
    })
    return roles;
  }
      // this.bsModalRef = this.modalService.show(RoleModalComponent);
  

}
