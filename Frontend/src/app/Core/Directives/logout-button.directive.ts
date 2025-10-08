// ==================================================
// Directiva: LogoutButtonDirective
// ==================================================
// Esta directiva agrega comportamiento de cierre de sesión al elemento sobre el cual se aplica.
// Al hacer clic, muestra una alerta de confirmación y ejecuta el cierre de sesión si el usuario lo confirma.

import { Directive, HostListener, inject, Input } from '@angular/core';
import { Router } from '@angular/router';
import { performLogout } from '../Utils/auth.utils';
import { AlertTotalService } from '../Service/alert-total.service';

@Directive({
	selector: '[appLogoutButton]',
	standalone: true
})
export class LogoutButtonDirective {

	// Inyección de servicios propios del proyecto
	private readonly alertService = inject(AlertTotalService);

	// Inyección de servicios nativos de Angular
	private readonly router = inject(Router);

	// Inputs principales de la directiva
	@Input() tokenKey: string = 'auth_token';

	// Evento host: escucha el clic en el elemento y gestiona el proceso de cierre de sesión
	@HostListener('click')
	onClick(): void {
		this.alertService.confirmLogout().then((result) => {
			if (result.isConfirmed) {
				performLogout(this.router, this.tokenKey);
			}
		});
	}
}
