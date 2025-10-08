// ==================================================
// Guard: roleGuard
// ==================================================
// Este guard protege rutas según el rol del usuario.
// Permite el acceso solo si el rol actual coincide con alguno de los roles esperados definidos en la ruta.

import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../Service/Auth/auth.service';

export const roleGuard: CanActivateFn = (route) => {

	// Inyección de servicios propios del proyecto
	const authService = inject(AuthService);

	// Inyección de servicios nativos de Angular
	const router = inject(Router);

	// Obtención de roles esperados desde la configuración de la ruta
	const expectedRoles = route.data['roles'] as Array<string>;
	const userRole = authService.getRole();

	// Si no hay rol de usuario, redirige al inicio
	if (!userRole) {
		router.navigate(['']);
		return false;
	}

	// Si el rol no coincide con los esperados, redirige a una página de acceso denegado
	if (!expectedRoles || expectedRoles.indexOf(userRole) === -1) {
		router.navigate(['/access-denied']);
		return false;
	}

	return true;
};
