import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AdminGuard } from './_guards/admin.guard';

import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailedResolver } from './_resolvers/member-details.resolver';


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
    { path:'Members/:username' , component: MemberDetailComponent,resolve: {member: MemberDetailedResolver}  },
    { path:'Member/edit' , component: MemberEditComponent,canDeactivate:[PreventUnsavedChangesGuard]  },
    { path:'lists' , component: ListsComponent,canActivate:[AuthGuard]  },
    { path:'messages', component:MessagesComponent,canActivate:[AuthGuard] },
    { path:'admin', component:AdminPanelComponent,canActivate: [AdminGuard] },
    { path:'**', component:HomeComponent,pathMatch :'full'}

  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
