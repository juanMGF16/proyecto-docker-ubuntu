import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../Service/Auth/auth.service';


export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const valid = authService.isAuthenticated();

  if (!valid) {
    router.navigate(['']);
    return false;
  }

  return true;
};
