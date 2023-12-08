import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { User } from '../models/user';
import { Observable, catchError, interval, take, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class UserApiService {
  apiUrl = 'https://localhost:7184/api/user';
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  user: User = new User(0, '', '');
  userList: User[] = [];
  router: any;

  constructor(private http: HttpClient) { }

  submitForm(firstName: string, lastName: string): boolean {
    this.user.firstName = firstName;
    this.user.lastName = lastName;
    this.user.id = 0;
    return this.createUser(this.user);
  }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }
   createUser(user: User): boolean {
    let isSuccess = false;
    this.http.post<User>(this.apiUrl, user).subscribe(data => {
      isSuccess = true;
      console.log('success', data, isSuccess);
      return isSuccess;
    },
      error => {
        console.log('oops', error);
        isSuccess = false;
      });
    return isSuccess;
  }
  async updateUser(user: User): Promise<boolean> {

    let isSuccess = false;
    this.http.put<User>(this.apiUrl, user).subscribe(data => {
      isSuccess = true;
      console.log('success', data)
    },
      error => {
        console.log('oops', error);
        isSuccess = false;
      });
    return isSuccess;

  }
  async deleteUser(id: number): Promise<boolean> {

    let isSuccess = false;
    this.http.delete<User>(`${this.apiUrl}/${id}`).subscribe(data => {
      isSuccess = true;
      console.log('success', data)
    },
      error => {
        console.log('oops', error);
        isSuccess = false;
      });
    return isSuccess;
  }
}
