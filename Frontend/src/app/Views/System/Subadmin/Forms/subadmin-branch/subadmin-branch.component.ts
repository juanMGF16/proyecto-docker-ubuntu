import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Validators } from '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { DataField, ShowInfoEdificationComponent } from '../../../../../Components/Shared/Forms/show-info-edification/show-info-edification.component';
import { EditField, UpdateInfoEdificationComponent } from "../../../../../Components/Shared/Modals/update-info-edification/update-info-edification.component";
import { BranchMod, BranchPartialUpdateMod } from '../../../../../Core/Models/System/BranchMod.model';
import { AuthService } from '../../../../../Core/Service/Auth/auth.service';
import { UserService } from '../../../../../Core/Service/SecurityModule/user.service';
import { BranchService } from '../../../../../Core/Service/System/branch.service';
import { mixedPhoneValidator } from '../../../../../Core/Utils/input-validators.util';

@Component({
	selector: 'app-subadmin-branch',
	standalone: true,
	imports: [CommonModule, ShowInfoEdificationComponent, UpdateInfoEdificationComponent],
	templateUrl: './subadmin-branch.component.html',
	styleUrl: './subadmin-branch.component.css'
})
export class SubadminBranchComponent implements OnInit {
	branchData = signal<BranchMod | null>(null);
	isEditModalOpen = signal(false);
	idUser: number = 0;

	// Campos configurables para el componente genérico a mostrar
	branchFields: DataField[] = [
		{ key: 'name', label: 'Nombre', icon: 'store' },
		{ key: 'address', label: 'Direccion', icon: 'home_pin' },
		{ key: 'phone', label: 'Teléfono', icon: 'phone' },
		{ key: 'companyName', label: 'Empresa', icon: 'apartment' },
	];

	// Campos para editar (con validadores)
	editFields: EditField[] = [
		{
			key: 'phone',
			label: 'Telefono',
			type: 'text',
			validators: [Validators.required, mixedPhoneValidator()]
		},
	];

	private branchService = inject(BranchService)
	private authService = inject(AuthService)

	ngOnInit(): void {
		this.loadBranchData();
	}

	loadBranchData(): void {
		const userIdString = this.authService.getIdUser();
		this.idUser = parseInt(userIdString, 10);
		if (this.idUser) {
			this.branchService.getByIdInCharge(this.idUser).subscribe({
				next: (branch) => this.branchData.set(branch),
				error: (error) => console.error('Error loading branch data:', error)
			});
		} else {
			console.warn('El usuario no tiene branch asociada.');
		}
	}

	// Servicio para guardar (usado por el modal genérico)
	saveBranch(branchData: BranchPartialUpdateMod): any {
		return this.branchService.partialUpdate(branchData);
	}

	get branch(): BranchMod | null {
		return this.branchData();
	}

	getCompanyInitials(): string {
		const name = this.branchData()?.name;
		return name ? name.charAt(0).toUpperCase() : 'E';
	}

	// Métodos para manejar el modal de edición
	openEditModal(): void {
		this.isEditModalOpen.set(true);
	}

	closeEditModal(): void {
		this.isEditModalOpen.set(false);
	}

	onCompanyUpdated(updatedBranch: BranchMod): void {
		this.branchData.set(updatedBranch);
		this.closeEditModal();
		this.loadBranchData();
	}

	// Métodos para manejar la eliminación
	openDeleteModal(): void {
		Swal.fire({
			title: '¿Estás seguro?',
			html: `
				<p style="font-size: 16px; margin: 15px 0;">
					Esta acción <strong>eliminará permanentemente</strong> la sucurusal:<br>
					<strong>"${this.branchData?.name}"</strong>
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
				this.deleteBranch();
			}
		});
	}

	private deleteBranch(): void {
		// this.branchService.delete(this.branchData.companyId, 2).subscribe({
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
