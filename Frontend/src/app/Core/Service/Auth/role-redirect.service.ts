import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class RoleRedirectService {

	constructor(private router: Router) { }

	private roleRoutes: Record<string, string> = {
		'SM_ACTION': '/securitymodule',
		'ADMINISTRADOR': '/admin',
		'SUBADMINISTRADOR': '/subadmin',
		'ENCARGADO_ZONA': '/areaManager',

	};

	redirectUser(role: string | null) {
		if (!role) {
			this.router.navigate(['/Login']);
			return;
		}

		const target = this.roleRoutes[role] || '/access-denied';
		this.router.navigate([target]);
	}
}
