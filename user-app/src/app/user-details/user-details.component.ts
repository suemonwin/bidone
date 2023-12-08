import { Component,Input } from '@angular/core';
import { User } from '../models/user';
import { UserApiService } from '../services/user-api.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.css'
})
export class UserDetailsComponent {
  
  userList: User[] = [];
  errorMessage: any;

  constructor(private userApiService: UserApiService, private route: ActivatedRoute) { }

  async ngOnInit() {
   
   // const userId = Number(this.route.snapshot.params['id']);

   this.userApiService.getAllUsers().subscribe(
      userList => {
        this.userList = userList
      },
      error => {
        this.errorMessage = error;
      }
    );
 
  }
}
