import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { HomeComponent } from './home/home.component';
import { UserDetailsComponent } from './user-details/user-details.component';

const routes: Routes =[
  {
      path: '',
      component: HomeComponent,
      title: 'Home Page'
  },
  {
      path: 'user',
      component: UserProfileComponent,
      title: 'User Page'
  },
  {
    path: 'userdetails/:id',
    component: UserDetailsComponent,
    title: 'User Page'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
