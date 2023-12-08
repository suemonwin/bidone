import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeComponent } from './home.component';
import { UserApiService } from '../services/user-api.service';
import { User } from '../models/user';
import { of } from 'rxjs';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let userApiService: jasmine.SpyObj<UserApiService>;
  const mockUserList: User[] = [
    { id: 1, firstName: 'John' , lastName: 'Paul'},
    { id: 2, firstName: 'Alice', lastName: 'Kim' }
    // Add more mock user data as needed
  ];

  beforeEach(async () => {
    userApiService = jasmine.createSpyObj('UserApiService', ['getAllUsers']);

    await TestBed.configureTestingModule({
      declarations: [HomeComponent],
      providers: [{ provide: UserApiService, useValue: userApiService }]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should retrieve user list on initialization', () => {
    userApiService.getAllUsers.and.returnValue(of(mockUserList));

    fixture.detectChanges();
    expect(component.userList).toEqual(mockUserList);
  });

  it('should handle error when user list retrieval fails', () => {
    const errorMessage = 'Error fetching users';
    userApiService.getAllUsers.and.throwError(errorMessage);

    fixture.detectChanges();
    expect(component.errorMessage).toEqual(errorMessage);
  });

});
