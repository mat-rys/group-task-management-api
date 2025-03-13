import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  tokenKey: string = ''

  constructor(private router: Router) { }

  //sprawdzenie tokena z lokalnej pamięci
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // Zapis tokena po logowaniu
  setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  // Usunięcie tokena przy wylogowaniu
  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/login']);
  }

  // Sprawdzenie czy użytkownik jest zalogowany
  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}
