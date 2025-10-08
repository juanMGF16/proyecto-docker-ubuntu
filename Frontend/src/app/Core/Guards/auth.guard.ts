// ==================================================
// Guard: authGuard
// ==================================================
// Este guard protege rutas que requieren autenticación.
// Verifica si el usuario ha iniciado sesión y, si no es así, redirige a la ruta raíz.

import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../Service/Auth/auth.service';

export const authGuard: CanActivateFn = () => {

	// Inyección de servicios propios del proyecto
	const authService = inject(AuthService);

	// Inyección de servicios nativos de Angular
	const router = inject(Router);

	// Verificación del estado de autenticación
	const valid = authService.isAuthenticated();

	// Redirige al inicio si el usuario no está autenticado
	if (!valid) {
		router.navigate(['']);
		return false;
	}

	return true;
};
