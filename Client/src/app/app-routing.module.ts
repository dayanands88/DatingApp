import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  
  { path:'Members',component:MemberListComponent,canActivate:[AuthGuard] },
  { path:'Members/:id' , component: MemberDetailComponent,canActivate:[AuthGuard] },
  { path:'Messages',component: MessagesComponent,canActivate:[AuthGuard]  },
  {path:'errors',component:TestErrorsComponent,canActivate:[AuthGuard] },
  { path: 'member-card',component:MemberCardComponent,canActivate:[AuthGuard] }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
