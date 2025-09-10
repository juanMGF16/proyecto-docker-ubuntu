import { Routes } from '@angular/router';
import { AdminScreenComponent } from './admin-screen/admin-screen.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { RegisterCompanyComponent } from './register-company/register-company.component';
import { AdminWelcomeComponent } from './admin-welcome/admin-welcome.component';
import { AdminProfileComponent } from './admin-profile/admin-profile.component';


export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    component: AdminScreenComponent,
    children: [
      { path: 'dashboard', component: AdminDashboardComponent },
      { path: 'welcome', component: AdminWelcomeComponent },
			{ path: 'register-company', component: RegisterCompanyComponent },
			{ path: 'profile', component: AdminProfileComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  }
];
