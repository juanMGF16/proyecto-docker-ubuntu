// ==================================================
// Interceptor: authInterceptor
// ==================================================
// Este interceptor agrega el token de autenticación a todas las solicitudes HTTP salientes.
// Si el usuario tiene un token válido, se incluye en el encabezado `Authorization` para
// permitir el acceso a rutas protegidas en el backend.

import { inject } from '@angular/core';
import { HttpInterceptorFn, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { AuthService } from '../Service/Auth/auth.service';

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn) => {

	// Inyección de servicios propios del proyecto
	const authService = inject(AuthService);

	// Obtiene el token de autenticación actual
	const token = authService.getToken();

	// Si existe un token, se clona la solicitud agregando el encabezado Authorization
	if (token) {
		req = req.clone({
			setHeaders: {
				Authorization: `Bearer ${token}`
			}
		});
	}

	// Continúa con el flujo de la solicitud HTTP
	return next(req);
};
