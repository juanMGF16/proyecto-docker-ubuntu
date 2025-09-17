import { Directive, HostListener, inject, Input } from '@angular/core';
import { Router } from '@angular/router';
import { confirmLogout } from '../Utils/alerts.util';
import { performLogout } from '../Utils/auth.util';


@Directive({
	selector: '[appLogoutButton]',
	standalone: true
})
export class LogoutButtonDirective {
	private router = inject(Router);

	@Input() tokenKey: string = 'auth_token';

	@HostListener('click')
	onClick(): void {
		confirmLogout().then((result) => {
			if (result.isConfirmed) {
				performLogout(this.router, this.tokenKey);
			}
		});
	}
}
