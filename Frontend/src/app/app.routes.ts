import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegistrationComponent } from './registration/register/registration.component';
import { TaskComponent } from './tasks/task/task.component';
import { authGuard } from './security/auth.guard';

export const routes: Routes = [
    {path: 'login', component: LoginComponent},
    {path: 'registration', component: RegistrationComponent},
    {path: 'tasks', component: TaskComponent, canActivate: [authGuard]},
    {path: '**', redirectTo: 'login'}
];
