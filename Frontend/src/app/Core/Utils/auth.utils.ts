// ==================================================
// Funciones de autenticación y control de sesión
// ==================================================
// Conjunto de utilidades relacionadas con la gestión de roles, autenticación y cierre de sesión.

import { Router } from "@angular/router";
import { AlertTotalService } from "../Service/alert-total.service";

// Verifica si el rol proporcionado corresponde a un administrador
export function isAdminRole(role: string | null | undefined): boolean {
	return (role || '') === 'SM_ACTION';
}

// Ejecuta el proceso de cierre de sesión eliminando el token y redirigiendo al login.
// Puede mostrar una notificación si se provee el servicio de alertas.
export function performLogout(router: Router, tokenKey: string = 'auth_token', alertService?: AlertTotalService) {
	localStorage.removeItem(tokenKey);

	if (alertService) {
		alertService.toast("Sesión cerrada correctamente", 'success');
	}

	router.navigate(['/Login']);
}

// Variante simplificada de performLogout que no depende de la inyección directa del servicio
export function performLogoutWithAlert(router: Router, tokenKey: string = 'auth_token') {
	performLogout(router, tokenKey);
}
