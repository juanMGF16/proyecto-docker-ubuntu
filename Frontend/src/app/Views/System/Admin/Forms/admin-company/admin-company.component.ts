import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Validators } from '@angular/forms';
import { DataField, ShowInfoEdificationComponent } from '../../../../../Components/Shared/Forms/show-info-edification/show-info-edification.component';
import { EditField, UpdateInfoEdificationComponent } from "../../../../../Components/Shared/Modals/update-info-edification/update-info-edification.component";
import { CompanyMod, CompanyPartialUpdateMod } from '../../../../../Core/Models/System/CompanyMod.model';
import { UserService } from '../../../../../Core/Service/SecurityModule/user.service';
import { CompanyService } from '../../../../../Core/Service/System/company.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { UserHasCompanyMod } from '../../../../../Core/Models/SecurityModule/UserMod.model';

@Component({
	selector: 'app-admin-company',
	standalone: true,
	imports: [CommonModule, ShowInfoEdificationComponent, UpdateInfoEdificationComponent],
	templateUrl: './admin-company.component.html',
	styleUrl: './admin-company.component.css'
})
export class AdminCompanyComponent implements OnInit {
	companyData = signal<CompanyMod | null>(null);
	isEditModalOpen = signal(false);

	// Campos configurables para el componente genérico a mostrar
	companyFields: DataField[] = [
		{ key: 'name', label: 'Nombre Comercial', icon: 'store' },
		{ key: 'businessName', label: 'Razón Social', icon: 'corporate_fare' },
		{ key: 'nit', label: 'NIT', icon: 'badge' },
		{ key: 'industryName', label: 'Industria', icon: 'category' },
		{ key: 'email', label: 'Email Corporativo', icon: 'email' },
		{ key: 'webSite', label: 'Sitio Web', icon: 'language' },
	];

	// Campos para editar (con validadores)
	editFields: EditField[] = [
		{
			key: 'email',
			label: 'Email Corporativo',
			type: 'email',
			validators: [Validators.required, Validators.email]
		},
		{
			key: 'webSite',
			label: 'Sitio Web',
			validators: [Validators.maxLength(200)]
		}
	];

	private companyService = inject(CompanyService)
	private userService = inject(UserService)
	private router = inject(Router);

	ngOnInit(): void {
		this.loadCompanyData();
	}

	loadCompanyData(): void {
		this.userService.hasCompany().subscribe({
			next: (data) => {
				if (data.hasCompany && data.companyId) {
					this.companyService.getById(data.companyId).subscribe({
						next: (company) => this.companyData.set(company),
						error: (error) => console.error('Error loading company data:', error)
					});
				} else {
					console.warn('El usuario no tiene compañía asociada.');
				}
			},
			error: (error) => console.error('Error verificando compañía:', error)
		});
	}

	// Servicio para guardar (usado por el modal genérico)
	saveCompany(companyData: CompanyPartialUpdateMod): any {
		return this.companyService.partialUpdate(companyData);
	}


	getCompanyInitials(): string {
		const name = this.companyData()?.name;
		return name ? name.charAt(0).toUpperCase() : 'E';
	}

	// Getter para obtener el valor de la señal (para usar en el template)
	get company(): CompanyMod | null {
		return this.companyData();
	}
	// Métodos para manejar el modal de edición
	openEditModal(): void {
		this.isEditModalOpen.set(true);
	}

	closeEditModal(): void {
		this.isEditModalOpen.set(false);
	}

	onCompanyUpdated(updatedCompany: CompanyMod): void {
		this.companyData.set(updatedCompany);
		this.closeEditModal();
		this.loadCompanyData();
	}

	// Métodos para manejar la eliminación
	openDeleteModal(): void {
		Swal.fire({
			title: '¿Estás seguro?',
			html: `
				<p style="font-size: 16px; margin: 15px 0;">
					Esta acción <strong>eliminará permanentemente</strong> la empresa:<br>
					<strong>"${this.company?.name}"</strong>
				</p>
				<p style="color: #e53e3e; font-size: 14px;">
					⚠️ <strong>Advertencia:</strong> Esta acción no se puede deshacer.
				</p>
			`,
			icon: 'warning',
			showCancelButton: true,
			confirmButtonColor: '#d33',
			cancelButtonColor: '#6b7280',
			confirmButtonText: 'Sí, eliminar empresa',
			cancelButtonText: 'Cancelar',
			reverseButtons: true,
			customClass: {
				confirmButton: 'swal2-confirm-delete',
				cancelButton: 'swal2-cancel-custom'
			}
		}).then((result) => {
			if (result.isConfirmed) {
				this.deleteCompany();
			}
		});
	}

	private deleteCompany(): void {
		this.userService.hasCompany().subscribe({
			next: (data: UserHasCompanyMod) => {
				if (data.hasCompany && data.companyId) {
					// Estrategia 2 para eliminación
					this.companyService.delete(data.companyId, 2).subscribe({
						next: () => {
							Swal.fire({
								title: '¡Empresa eliminada!',
								text: 'La empresa ha sido eliminada exitosamente',
								icon: 'success',
								confirmButtonColor: '#28a745',
								confirmButtonText: 'Aceptar',
							}).then(() => {
								// Redirigir al dashboard o recargar
								this.router.navigate(['/admin/dashboard']);
							});
						},
						error: (error) => {
							console.error('Error eliminando empresa:', error);
						}
					});
				} else {
					Swal.fire({
						title: 'Error',
						text: 'No se encontró la empresa a eliminar',
						icon: 'error',
						confirmButtonText: 'Entendido',
					});
				}
			},
			error: (error) => {
				console.error('Error obteniendo ID de empresa:', error);
			}
		});
	}
}
