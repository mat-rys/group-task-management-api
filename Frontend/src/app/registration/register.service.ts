import { Injectable } from '@angular/core';
import { RegistrationProfile } from './models/registration-profile';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(private http: HttpClient) { 
  }

  registerNewUser(newProfile: RegistrationProfile): Observable<any> {
    return this.http.post('https://localhost:7221/api/UserProfile/register', newProfile);
  }
}
