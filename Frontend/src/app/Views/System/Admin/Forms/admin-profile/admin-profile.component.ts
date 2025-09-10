import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../../../Core/Service/Auth/auth.service';
import { PersonService } from '../../../../../Core/Service/SecurityModule/person.service';
import { AdminEditProfileModalComponent } from '../../../../../Components/System/Admin/Modal/admin-edit-profile/admin-edit-profile.component';
import { AdminChangePasswordModalComponent } from '../../../../../Components/System/Admin/Modal/admin-change-password/admin-change-passwordcomponent';
import { catchError, of } from 'rxjs';
import { PersonMod } from '../../../../../Core/Models/SecurityModule/PersonMod.model';

@Component({
	selector: 'app-admin-profile',
	standalone: true,
	imports: [
		CommonModule,
		MatButtonModule,
		MatIconModule,
		MatTabsModule,
		RouterModule,
		AdminEditProfileModalComponent,
		AdminChangePasswordModalComponent
	],
	templateUrl: './admin-profile.component.html',
	styleUrls: ['./admin-profile.component.css']
})
export class AdminProfileComponent implements OnInit {
	private readonly authService = inject(AuthService);
	private readonly personService = inject(PersonService);

	// Signals
	user = signal<PersonMod | null>(null);
	isEditModalOpen = signal(false);
	isPasswordModalOpen = signal(false);
	lastLogin = signal<Date | null>(null);
	isLoading = signal(true);
	error = signal<string | null>(null);

	// Diccionario de tipos de documento
	private readonly documentTypes: Record<string, string> = {
		RC: 'Registro Civil',
		TI: 'Tarjeta de Identidad',
		CC: 'Cédula de Ciudadanía',
		CE: 'Cédula de Extranjería',
		NIT: 'NIT',
		PP: 'Pasaporte'
	};

	ngOnInit(): void {
		this.loadUserData();
	}

	private loadUserData(): void {
		const personIdString = this.authService.getIdPerson();

		if (!personIdString) {
			this.error.set('No se pudo obtener el ID de Person');
			this.isLoading.set(false);
			return;
		}

		// Convertir string a number con validación
		const personId = Number(personIdString);

		if (isNaN(personId) || personId <= 0) {
			this.error.set('ID de Person inválido');
			this.isLoading.set(false);
			return;
		}

		this.personService.getById(personId).pipe(
			catchError((error) => {
				console.error('Error al cargar datos:', error);
				this.error.set('Error al cargar los datos de Person');
				this.isLoading.set(false);
				return of(null);
			})
		).subscribe({
			next: (userData) => {
				if (userData) {
					this.user.set(userData);
					// Manejo de lastLogin: si no viene de la API, se asigna la fecha actual
					this.lastLogin.set(
						userData.lastLogin ? new Date(userData.lastLogin) : new Date()
					);
				} else {
					this.error.set('No se encontraron datos del usuario');
				}
				this.isLoading.set(false);
			},
			error: () => {
				this.isLoading.set(false);
			}
		});
	}

	// Métodos para manejar el modal de edición
	openEditModal(): void {
		this.isEditModalOpen.set(true);
	}

	closeEditModal(): void {
		this.isEditModalOpen.set(false);
	}

	onProfileUpdated(updatedUser: PersonMod): void {
		this.user.set(updatedUser);
		this.closeEditModal();
		this.loadUserData(); // Recargar datos actualizados
	}

	// Métodos para manejar el modal de contraseña
	openPasswordModal(): void {
		this.isPasswordModalOpen.set(true);
	}

	closePasswordModal(): void {
		this.isPasswordModalOpen.set(false);
	}

	onPasswordChanged(): void {
		this.closePasswordModal();
	}

	// Método de utilidad para obtener las iniciales del usuario
	getUserInitials(): string {
		const user = this.user();
		if (!user) return 'U';

		const name = user.name?.charAt(0) || '';
		const lastName = user.lastName?.charAt(0) || '';
		return (name + lastName).toUpperCase() || 'U';
	}

	// Método para formatear el rol del usuario
	getFormattedRole(): string {
		const role = this.authService.getRole();
		return role ? role.replace('_', ' ').toUpperCase() : 'USUARIO';
	}

	// Método para traducir tipo de documento
	getDocumentLabel(code: string | null | undefined): string {
		if (!code) return 'N/A';
		return this.documentTypes[code] ?? code;
	}
}
