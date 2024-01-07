import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'; // Import the 'HttpClient' class
import { environment } from 'src/environments/environment.development';
import { Login } from '../shared/models/login';
import { Register } from '../shared/models/register';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private http: HttpClient) {}

  login(model: Login) {
    return this.http.post(`${environment.appUrl}/api/account/login`, model);
  }

  register(model: Register) {
    return this.http.post(`${environment.appUrl}/api/account/register`, model);
  }
}
