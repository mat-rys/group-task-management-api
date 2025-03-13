import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginCredentials } from './models/login-credentials';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  getAuthToken(creditentials: LoginCredentials): Observable<any>{
    return this.http.post('https://localhost:7221/api/UserProfile/login', creditentials)
  }
}
