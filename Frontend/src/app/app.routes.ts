import { Routes } from '@angular/router';
import { authGuard } from './Core/Guards/auth.guard';
import { LoginComponent } from './Views/Auth/login/login.component';
import { RegisterComponent } from './Views/Auth/register/register.component';
import { AccessDeniedComponent } from './Components/Shared/access-denied/access-denied.component';
import { PageNotFoundComponent } from './Components/Shared/page-not-found/page-not-found.component';
import { roleGuard } from './Core/Guards/role.guard';
import { LandingComponent } from './Views/System/Landing/landing.component';

export const routes: Routes = [
  {path: '', component: LandingComponent},

  // -----------------------
  // Auth
  // -----------------------
  {path: 'Login', component: LoginComponent},
  { path: 'Register', component: RegisterComponent},
	{ path: 'access-denied', component: AccessDeniedComponent },

	// -----------------------
  // SecurityModule
  // -----------------------
	{
    path: 'securitymodule',
    loadChildren: () => import('./Views/SecurityModule/sm.routes').then(m => m.SM_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { roles: ['SM_ACTION'] }
  },

	// -----------------------
  // System - Admin
  // -----------------------
  {
    path: 'admin',
    loadChildren: () => import('./Views/System/Admin/admin.routes').then(m => m.ADMIN_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { roles: ['ADMINISTRADOR'] }
  },

	// -----------------------
  // Redirecciones
  // -----------------------
  { path: '', redirectTo: '', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent },
];
