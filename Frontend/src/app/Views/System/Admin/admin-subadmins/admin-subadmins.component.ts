import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { Router } from '@angular/router';
import { SubadminFilterPipe } from '../../../../Core/Pipes/subadmins-filter.pipe';
import { FormsModule } from '@angular/forms'; // Importar FormsModule

export interface SubAdmin {
	id: number;
	name: string;
	lastName: string;
	phone: string;
	email: string;
	documentType: string;
	documentNumber: string;
	branch: string;
	branchId: number;
}

@Component({
	selector: 'app-admin-subadmins',
	standalone: true,
	imports: [CommonModule, MatButtonModule, MatIconModule, MatTableModule, SubadminFilterPipe, FormsModule ],
	templateUrl: './admin-subadmins.component.html',
	styleUrls: ['../../../../Components/Shared/Styles/modal-shared.css','./admin-subadmins.component.css']
})
export class AdminSubadminsComponent {
	private router = inject(Router);

	selectedSubAdmin = signal<SubAdmin | null>(null);
	isModalOpen = signal(false);
	searchText: string = '';

	// Datos hardcodeados
	subadmins: SubAdmin[] = [
		{
			id: 1,
			name: 'María',
			lastName: 'González Rodríguez',
			phone: '+57 312 345 6789',
			email: 'maria.gonzalez@empresa.com',
			documentType: 'Cédula de Ciudadanía',
			documentNumber: '1234567890',
			branch: 'Sucursal Central',
			branchId: 1
		},
		{
			id: 2,
			name: 'Carlos',
			lastName: 'López Martínez',
			phone: '+57 315 987 6543',
			email: 'carlos.lopez@empresa.com',
			documentType: 'Cédula de Ciudadanía',
			documentNumber: '0987654321',
			branch: 'Sucursal Norte',
			branchId: 2
		},
		{
			id: 3,
			name: 'Ana',
			lastName: 'Martínez Silva',
			phone: '+57 320 123 4567',
			email: 'ana.martinez@empresa.com',
			documentType: 'Cédula de Ciudadanía',
			documentNumber: '1122334455',
			branch: 'Sucursal Sur',
			branchId: 3
		}
	];

	displayedColumns: string[] = ['name', 'phone', 'branch', 'actions'];

	viewDetails(subadmin: SubAdmin): void {
		this.selectedSubAdmin.set(subadmin);
		this.isModalOpen.set(true);
	}

	closeModal(): void {
		this.isModalOpen.set(false);
		this.selectedSubAdmin.set(null);
	}

	navigateToBranches(): void {
		this.router.navigate(['/admin/register-branch']);
	}

	get hasSubadmins(): boolean {
		return this.subadmins.length > 0;
	}
}
