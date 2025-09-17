import { Router } from "@angular/router";
import { successMessage } from "./alerts.util";

export function isAdminRole(role: string | null | undefined): boolean {
	return (role || '') === 'SM_ACTION';
}

export function performLogout(router: Router, tokenKey: string = 'auth_token') {
	localStorage.removeItem(tokenKey);
	successMessage("Sesión cerrada");
	router.navigate(['/Login']);
}
