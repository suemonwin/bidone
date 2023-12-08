import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { UserProfileComponent } from './user-profile.component';
import { UserApiService } from '../services/user-api.service';
import { ValidatorService } from '../services/validator.service';

describe('UserProfileComponent', () => {
  let component: UserProfileComponent;
  let fixture: ComponentFixture<UserProfileComponent>;
  let mockUserApiService: jasmine.SpyObj<UserApiService>;
  let mockValidatorService: jasmine.SpyObj<ValidatorService>;

  beforeEach(async () => {
    mockUserApiService = jasmine.createSpyObj('UserApiService', ['submitForm']);
    mockValidatorService = jasmine.createSpyObj('ValidatorService', ['specialCharacterValidator']);

    await TestBed.configureTestingModule({
      declarations: [UserProfileComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: UserApiService, useValue: mockUserApiService },
        { provide: ValidatorService, useValue: mockValidatorService },
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { paramMap: { get: () => '1' } } } // Mock ActivatedRoute snapshot
        }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call userApiService.submitForm on form submission', () => {
    // Set form values
    component.userForm.setValue({ firstName: 'John', lastName: 'Doe' });

    // Simulate form submission
    component.submitForm();

    // Check if userApiService.submitForm was called with the expected values
    expect(mockUserApiService.submitForm).toHaveBeenCalledWith('John', 'Doe');
  });

  // Add more test cases to cover other scenarios
});
