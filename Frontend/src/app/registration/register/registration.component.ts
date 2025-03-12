import { Component } from '@angular/core';
import { RegistrationProfile } from '../models/registration-profile';
import { FormsModule } from '@angular/forms';
import { RegisterService } from '../register.service';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.scss'
})
export class RegistrationComponent {

  constructor(private registerService: RegisterService){
  }
  
  newUser: RegistrationProfile = {
    username: '',
    password: '',
    role: ''
  };

  creatAccount(newProfile: RegistrationProfile){
    console.log('New user profile created:', newProfile);
    this.registerService.registerNewUser(newProfile).subscribe({
      next: (response) => {
        console.log('User successfully registered', response);
      },
      error: (error) => {
        console.error('Error registering user:', error);
      }
    });
  }
}
