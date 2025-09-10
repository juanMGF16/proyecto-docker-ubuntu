// admin-branch.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { ZoneFilterPipe } from '../../../../Core/Pipes/zone-filter.pipe';

export interface Zone {
	id: number;
	name: string;
	encargado: string;
	totalItems: number;
}

interface SubAdministrador {
	nombre: string;
	apellidos: string;
	celular: string;
	email?: string;
}

@Component({
	selector: 'app-admin-branch',
	standalone: true,
	imports: [
		CommonModule,
		MatIconModule,
		MatCardModule,
		MatButtonModule,
		FormsModule,
		MatInputModule,
		ZoneFilterPipe
	],
	templateUrl: './admin-branch.component.html',
	styleUrl: './admin-branch.component.css'
})
export class AdminBranchComponent {
	sucursal = {
		id: 1,
		nombre: 'Sucursal Central',
		direccion: 'Av. Principal #123, Ciudad',
		telefono: '+1 (555) 123-4567',
		inventariosTotales: 42,
		totalItems: 287
	};

	subAdministrador: SubAdministrador = {
		nombre: 'María',
		apellidos: 'González Rodríguez',
		celular: '+1 (555) 987-6543',
		email: 'maria.gonzalez@empresa.com'
	};

	// zonas: Zone[] = [];
	zonas: Zone[] = [
		{ id: 1, name: 'Zona de Almacén', encargado: 'Carlos López', totalItems: 89 },
		{ id: 2, name: 'Zona de Exhibición', encargado: 'Ana Martínez', totalItems: 67 },
		{ id: 3, name: 'Zona de Oficinas', encargado: 'Pedro Sánchez', totalItems: 45 },
		{ id: 4, name: 'Zona de Servicios', encargado: 'Laura Díaz', totalItems: 56 },
		{ id: 5, name: 'Zona Exterior', encargado: 'Jorge Ramírez', totalItems: 30 }
	];

	searchText: string = '';

	// Getter para verificar si hay zonas
	get hasZones(): boolean {
		return this.zonas.length > 0;
	}

	onDeleteSucursal() {
		console.log('Eliminar/Deshabilitar sucursal:', this.sucursal.id);
		// Lógica para eliminar/deshabilitar
	}
}
