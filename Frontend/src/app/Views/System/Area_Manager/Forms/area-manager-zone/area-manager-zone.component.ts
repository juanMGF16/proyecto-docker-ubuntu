import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { DataField, ShowInfoEdificationComponent } from '../../../../../Components/Shared/Forms/show-info-edification/show-info-edification.component';
import { EditField, UpdateInfoEdificationComponent } from "../../../../../Components/Shared/Modals/update-info-edification/update-info-edification.component";
import { ZoneMod, ZonePartialUpdateMod } from '../../../../../Core/Models/System/ZoneMod.model';
import { AuthService } from '../../../../../Core/Service/Auth/auth.service';
import { ZoneService } from '../../../../../Core/Service/System/zone.service';

@Component({
	selector: 'app-area-manager-zone',
	standalone: true,
	imports: [CommonModule, ShowInfoEdificationComponent, UpdateInfoEdificationComponent],
	templateUrl: './area-manager-zone.component.html',
	styleUrl: './area-manager-zone.component.css'
})
export class AreaManagerZoneComponent implements OnInit {
	zoneData = signal<ZoneMod | null>(null);
	isEditModalOpen = signal(false);
	idUser: number = 0;

	// Campos configurables para el componente genérico a mostrar
	zoneFields: DataField[] = [
		{ key: 'name', label: 'Nombre', icon: 'store' },
		{ key: 'description', label: 'Descripción', icon: 'description' },
		{ key: 'branchName', label: 'Sucursal', icon: 'store' },
	];

	// Campos para editar (con validadores)
	editFields: EditField[] = [
		{
			key: 'name',
			label: 'Nombre',
			type: 'text',
			validators: [Validators.required, Validators.minLength(3)]
		},
		{
			key: 'description',
			label: 'Descripción',
			type: 'text',
			validators: [Validators.required, Validators.minLength(5)]
		}
	];

	private zoneService = inject(ZoneService)
	private authService = inject(AuthService)

	ngOnInit(): void {
		this.loadZoneData();
	}

	loadZoneData(): void {
		const userIdString = this.authService.getIdUser();
		this.idUser = parseInt(userIdString, 10);
		if (this.idUser) {
			this.zoneService.getByIdAreaManager(this.idUser).subscribe({
				next: (zone) => this.zoneData.set(zone),
				error: (error) => console.error('Error loading zone data:', error)
			});
		} else {
			console.warn('El usuario no tiene zone asociada.');
		}
	}

	// Servicio para guardar (usado por el modal genérico)
	saveZone(zoneData: ZonePartialUpdateMod): any {
		return this.zoneService.partialUpdate(zoneData);
	}

	get zone(): ZoneMod | null {
		return this.zoneData();
	}

	getBranchInitials(): string {
		const name = this.zoneData()?.name;
		return name ? name.charAt(0).toUpperCase() : 'E';
	}

	// Métodos para manejar el modal de edición
	openEditModal(): void {
		this.isEditModalOpen.set(true);
	}

	closeEditModal(): void {
		this.isEditModalOpen.set(false);
	}

	onCompanyUpdated(updatedZone: ZoneMod): void {
		this.zoneData.set(updatedZone);
		this.closeEditModal();
		this.loadZoneData();
	}

	// Métodos para manejar la eliminación
	openDeleteModal(): void {
		Swal.fire({
			title: '¿Estás seguro?',
			html: `
				<p style="font-size: 16px; margin: 15px 0;">
					Esta acción <strong>eliminará permanentemente</strong> la zona:<br>
					<strong>"${this.zoneData?.name}"</strong>
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
				this.deleteZone();
			}
		});
	}

	private deleteZone(): void {
		// this.zoneService.delete(this.zoneData.companyId, 2).subscribe({
		// 	next: () => {
		// 		Swal.fire({
		// 			title: '¡Empresa eliminada!',
		// 			text: 'La empresa ha sido eliminada exitosamente',
		// 			icon: 'success',
		// 			confirmButtonColor: '#28a745',
		// 			confirmButtonText: 'Aceptar',
		// 		}).then(() => {
		// 			// Redirigir al dashboard o recargar
		// 			this.router.navigate(['/admin/dashboard']);
		// 		});
		// 	},
		// 	error: (error) => {
		// 		console.error('Error eliminando empresa:', error);
		// 	}
		// });
		console.log("Por implementar")
	}
}
