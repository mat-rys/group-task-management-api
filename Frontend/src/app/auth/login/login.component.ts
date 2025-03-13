import { Component } from '@angular/core';
import { LoginCredentials } from '../models/login-credentials';
import { FormsModule } from '@angular/forms';
import { LoginService } from '../login.service';
import { AuthService } from '../../security/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  creditentials: LoginCredentials = {
    username: '',
    password: ''
  }

  constructor(private loginService: LoginService, 
    private authService: AuthService,
    private router: Router){}

  loginUser(providedCreditentials: LoginCredentials){
      this.loginService.getAuthToken(providedCreditentials)
      .subscribe({
        next: (response) => {
          console.log('User successfully login', response);
          const token = response.token; 
          this.authService.setToken(token);
          this.router.navigate(['/tasks'])
        },
        error: (error) => {
          console.error('Error logging user:', error);
        }
      });
  }
}
