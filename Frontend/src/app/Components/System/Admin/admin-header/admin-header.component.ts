import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from "@angular/material/toolbar";
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../Core/Service/Auth/auth.service';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatBadgeModule } from '@angular/material/badge';
import { confirmLogout, successMessage } from '../../../../Core/Utils/alerts.util';
import { LogoutButtonDirective } from "../../../../Core/Directives/logout-button.directive";

@Component({
	selector: 'app-admin-header',
	imports: [
    CommonModule,
    MatToolbarModule,
    MatIconModule,
    MatMenuModule,
    MatButtonModule,
    MatDividerModule,
    RouterLink,
    MatBadgeModule,
    LogoutButtonDirective
],
	standalone: true,
	templateUrl: './admin-header.component.html',
	styleUrls: ['../../../Shared/Styles/header-shared.css','./admin-header.component.css']
})
export class AdminHeaderComponent {
	@Input() redirectUrl: string = '';
	@Input() isFixed: boolean = true;
	@Input() hasCompany: boolean | null = false; // Nuevo input
	@Output() toggleSidebar = new EventEmitter<void>();

	notificationCount = 1;
	notificationRead = false;

	constructor(
		private authService: AuthService,
		private router: Router
	) { }

	get logoRedirectUrl(): string {
		return this.hasCompany ? '/admin/dashboard' : '/admin/welcome';
	}

	onToggleSidebar(): void {
		this.toggleSidebar.emit();
	}

	get username(): string {
		return this.authService.getUsername();
	}

	get role(): string {
		return this.authService.getRole();
	}

	goToProfile(): void {
		this.router.navigate(['/admin/profile']);
	}

	goToCompany(): void {
		this.router.navigate(['/admin/empresa/configuracion']);
	}
}
