import { Component } from '@angular/core';
import { AuthService } from '../../security/auth.service';

@Component({
  selector: 'app-task',
  standalone: true,
  imports: [],
  templateUrl: './task.component.html',
  styleUrl: './task.component.scss'
})
export class TaskComponent {

  constructor(private authService: AuthService){
  }

  logout(){
    console.log("hey")
    this.authService.logout();
  }

}
