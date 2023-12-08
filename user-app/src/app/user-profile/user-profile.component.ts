import { Component } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { UserApiService } from '../services/user-api.service';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ValidatorService } from '../services/validator.service';
import { HomeComponent } from '../home/home.component';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent {
  userForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private userApiService: UserApiService,
    private validatorService: ValidatorService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.userForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(100), this.validatorService.specialCharacterValidator()]],
      lastName: ['', [Validators.required, Validators.maxLength(100), this.validatorService.specialCharacterValidator()]]
    });
  }

   submitForm() {
    const fName = this.userForm.value.firstName ?? '';
    const lName = this.userForm.value.lastName ?? '';
    let isSuccess = this.userApiService.submitForm(fName, lName);
    if( isSuccess==true)
    {
      console.log(isSuccess);
    }    
    this.userForm.reset();  
    //window.location.reload(); 
    this.router.navigate(['']);   
  }
}
