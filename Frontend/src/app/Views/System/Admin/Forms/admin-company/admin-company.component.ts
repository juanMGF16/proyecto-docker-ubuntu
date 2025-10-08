import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DataField, ShowInfoEdificationComponent } from '../../../../../Components/Shared/Forms/show-info-edification/show-info-edification.component';
import { EditField, UpdateInfoEdificationComponent } from "../../../../../Components/Shared/Modals/update-info-edification/update-info-edification.component";
import { UserHasCompanyMod } from '../../../../../Core/Models/SecurityModule/UserMod.model';
import { CompanyMod, CompanyPartialUpdateMod } from '../../../../../Core/Models/System/CompanyMod.model';
import { AlertTotalService } from '../../../../../Core/Service/alert-total.service';
import { UserService } from '../../../../../Core/Service/SecurityModule/user.service';
import { CompanyService } from '../../../../../Core/Service/System/company.service';
import { lastValueFrom } from 'rxjs';

@Component({
	selector: 'app-admin-company',
	standalone: true,
	imports: [CommonModule, ShowInfoEdificationComponent, UpdateInfoEdificationComponent],
	templateUrl: './admin-company.component.html',
	styleUrl: './admin-company.component.css'
})
export class AdminCompanyComponent implements OnInit {

	// Inyección de servicios propios del proyecto
	private readonly companyService = inject(CompanyService)
	private readonly userService = inject(UserService)
	private readonly alertService = inject(AlertTotalService);

	// Inyección de servicios nativos de Angular
	private readonly router = inject(Router);

	// Signals para datos de la empresa y estado del modal
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
		this.alertService.custom({
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
		}).then((result) => {
			if (result.isConfirmed) {
				this.deleteCompany();
			}
		});
	}


	private async deleteCompany(): Promise<void> {
		try {
			await this.alertService.withLoading(
				async () => {
					// lastValueFrom lanza error si no hay valor, evitando undefined
					const data = await lastValueFrom(this.userService.hasCompany());

					if (!data.hasCompany || !data.companyId) {
						throw new Error('No se encontró la empresa a eliminar');
					}

					await lastValueFrom(this.companyService.delete(data.companyId, 2));

					return data;
				},
				{
					loadingTitle: 'Eliminando empresa...',
					loadingText: 'Procesando eliminación de la empresa',
					successTitle: '¡Empresa eliminada!',
					successText: 'La empresa ha sido eliminada exitosamente',
					errorTitle: 'Error',
					errorText: 'Ocurrió un problema al eliminar la empresa'
				}
			);

			this.router.navigate(['/admin/welcome']);

		} catch (error) {
			console.error('Error en deleteCompany:', error);
		}
	}
}
