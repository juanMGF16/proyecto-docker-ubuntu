import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../Service/Auth/auth.service';

export const roleGuard: CanActivateFn = (route) => {
	const authService = inject(AuthService);
	const router = inject(Router);

	const expectedRoles = route.data['roles'] as Array<string>;
	const userRole = authService.getRole();

	// console.log('[ROLE GUARD] Rol esperado:', expectedRoles, 'Rol usuario:', userRole);

	if (!userRole) {
		router.navigate(['']);
		return false;
	}

	if (!expectedRoles || expectedRoles.indexOf(userRole) === -1) {
		router.navigate(['/access-denied']);
		return false;
	}

	return true;
};
