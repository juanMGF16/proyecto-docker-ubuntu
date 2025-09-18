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
<<<<<<< HEAD
			{ path: 'company', component: AdminCompanyComponent },
			{ path: 'register-branch', component: RegisterBranchComponent },
			{ path: 'branch/:id', component: AdminBranchComponent },
			{ path: 'subadmins-list', component: AdminSubadminsComponent },
=======
>>>>>>> parent of 845d2803 (solucion de errores)
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  }
];
