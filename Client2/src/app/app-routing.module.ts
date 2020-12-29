import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';

import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';


const routes: Routes = [
  // {
  //   path : '',
  //   runGuardsAndResolvers :'always',
  //   canActivate:[AuthGuard],
  //   children:[
  //     { path:'Members',component:MemberListComponent },
  //     { path:'Members/:id' , component: MemberListComponent }
      
  //   ]

  // },
    {path:'',component: HomeComponent},
    { path: 'Members',component:MemberListComponent,canActivate:[AuthGuard] },
    { path:'Members/:username' , component: MemberDetailComponent  },
    { path:'Member/edit' , component: MemberEditComponent,canDeactivate:[PreventUnsavedChangesGuard]  },
    { path:'lists' , component: ListsComponent,canActivate:[AuthGuard]  },
    { path:'messages', component:MessagesComponent,canActivate:[AuthGuard] },
    { path:'**', component:HomeComponent,pathMatch :'full'}

  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
