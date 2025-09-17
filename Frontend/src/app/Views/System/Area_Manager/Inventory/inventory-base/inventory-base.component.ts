import { CommonModule } from '@angular/common';
import { Component, OnInit, computed, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatDivider } from "@angular/material/divider";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from "@angular/material/input";
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { LoaderComponent } from '../../../../../Components/Shared/app-loader/app-loader.component';
import { InventoryItemTable, InventoryTableComponent } from '../../../../../Components/System/Area_Manager/Inventory/inventory-table/inventory-table.component';
import { ImportExcelComponent } from '../../../../../Components/System/Area_Manager/Modals/import-excel/import-excel.component';
import { infoMessage, successMessage } from '../../../../../Core/Utils/alerts.util';
import { InventoryBaseFilterPipe } from '../../../../../Core/Pipes/inventory-base-filter.pipe';
import { StateInventoryBasePipe } from "../../../../../Core/Pipes/state-inventory-base.pipe";
import { CategoryInventoryBasePipe } from "../../../../../Core/Pipes/category-inventory-base.pipe";

@Component({
	selector: 'app-inventory-base',
	standalone: true,
	imports: [
		CommonModule,
		FormsModule,
		// Angular Material
		MatTableModule,
		MatIconModule,
		MatMenuModule,
		MatButtonModule,
		MatChipsModule,
		MatFormFieldModule,
		MatInputModule,
		MatPaginatorModule,
		MatDivider,
		// Componentes personalizados
		LoaderComponent,
		ImportExcelComponent,
		InventoryTableComponent,
		// Pipes
		StateInventoryBasePipe,
		CategoryInventoryBasePipe
	],
	templateUrl: './inventory-base.component.html',
	styleUrls: ['./inventory-base.component.css']
})
export class InventoryBaseComponent implements OnInit {
	// Señales para el estado del componente
	readonly loading = signal(true);
	readonly error = signal(false);
	readonly errorMessage = signal('');
	readonly hasInventoryData = signal(false);
	readonly showImportModal = signal(false);

	// Señales para filtros y paginación
	readonly searchText = signal('');
	readonly categoryFilter = signal('all');
	readonly statusFilter = signal('all');
	readonly pageIndex = signal(0);
	readonly pageSize = signal(10);

	// Datos
	readonly zoneName = signal('Tu Zona');
	readonly allItems = signal<InventoryItemTable[]>([]);

	// Datos de ejemplo
	private readonly mockInventoryData = {
		items: [
			{ id: '1', code: 'TECH-001', name: 'Monitor LED 24"', description: 'Monitor LED de 24 pulgadas', category: 'technology', state: 'En orden' },
			{ id: '2', code: 'TECH-002', name: 'Monitor LED 27"', description: 'Monitor LED de 27 pulgadas', category: 'technology', state: 'En orden' },
			{ id: '3', code: 'FURN-045', name: 'Silla Ejecutiva', description: 'Silla ergonómica', category: 'furniture', state: 'Perdido' },
			{ id: '4', code: 'SUPP-112', name: 'Resma A4', description: 'Papel tamaño A4', category: 'supplies', state: 'Dañado' },
			{ id: '5', code: 'TECH-003', name: 'Teclado Mecánico', description: 'Teclado RGB', category: 'technology', state: 'Reparación' },
		],
		zoneName: 'Zona Norte',
		totalCount: 0
	} as const;

	// Valores computados


	readonly paginatedItems = computed(() => {
		const startIndex = this.pageIndex() * this.pageSize();
		const endIndex = startIndex + this.pageSize();
		// Usamos el pipe para filtrar los datos
		const filteredData = new InventoryBaseFilterPipe().transform(
			this.allItems(),
			this.searchText(),
			this.categoryFilter(),
			this.statusFilter()
		);
		return filteredData.slice(startIndex, endIndex);
	});

	readonly totalItems = computed(() => {
		// Aplicamos el mismo filtro para obtener el total
		return new InventoryBaseFilterPipe().transform(
			this.allItems(),
			this.searchText(),
			this.categoryFilter(),
			this.statusFilter()
		).length;
	});

	// Opciones para filtros
	readonly categories = computed(() =>
		['all', ...new Set(this.allItems().map(item => item.category))]
	);

	readonly statuses = computed(() =>
		['all', ...new Set(this.allItems().map(item => item.state))]
	);

	ngOnInit(): void {
		this.loadInventoryData();
	}

	loadInventoryData(): void {
		this.loading.set(true);
		this.error.set(false);

		// Simular carga asíncrona
		setTimeout(() => {
			try {
				this.zoneName.set(this.mockInventoryData.zoneName);
				this.allItems.set([...this.mockInventoryData.items]);
				this.hasInventoryData.set(this.allItems().length > 0);
				this.loading.set(false);
			} catch (error) {
				this.handleError('Error al cargar el inventario base');
			}
		}, 1500);
	}

	// Métodos de filtrado
	filterByCategory(category: string): void {
		this.categoryFilter.set(category);
		this.resetPagination();
	}

	filterByStatus(status: string): void {
		this.statusFilter.set(status);
		this.resetPagination();
	}

	clearFilters(): void {
		this.searchText.set('');
		this.categoryFilter.set('all');
		this.statusFilter.set('all');
		this.resetPagination();
	}

	onSearchChange(): void {
		this.resetPagination();
	}

	// Paginación
	onPageChange(event: PageEvent): void {
		this.pageIndex.set(event.pageIndex);
		this.pageSize.set(event.pageSize);
	}

	getDisplayedRange(): string {
		const total = this.totalItems();
		if (total === 0) return "0 - 0";

		const start = this.pageIndex() * this.pageSize() + 1;
		const end = Math.min((this.pageIndex() + 1) * this.pageSize(), total);
		return `${start} - ${end}`;
	}

	private resetPagination(): void {
		this.pageIndex.set(0);
	}

	// Gestión de modales
	openImportModal(): void {
		this.showImportModal.set(true);
	}

	closeImportModal(): void {
		this.showImportModal.set(false);
	}

	// Procesamiento de archivos
	processExcelImport(file: File): void {
		this.loading.set(true);
		this.closeImportModal();

		// Simular procesamiento
		setTimeout(() => {
			this.loading.set(false);
			successMessage('Importación exitosa', 'Inventario base importado correctamente');
			this.hasInventoryData.set(true);
		}, 2000);
	}

	downloadTemplate(): void {
		infoMessage('Descargando...', 'Se está descargando la plantilla de Excel');

		setTimeout(() => {
			const link = document.createElement('a');
			link.href = '/assets/templates/inventory-template.xlsx';
			link.download = 'plantilla-inventario-base.xlsx';
			document.body.appendChild(link);
			link.click();
			document.body.removeChild(link);
			successMessage('Descarga completa', 'Plantilla descargada correctamente');
		}, 1000);
	}

	// Acciones de la tabla
	onEditItem(item: InventoryItemTable): void {
		console.log('Edit item:', item);
		// TODO: Implementar lógica de edición
	}

	onDeleteItem(item: InventoryItemTable): void {
		console.log('Delete item:', item);
		// TODO: Implementar lógica de eliminación con confirmación
	}

	exportToExcel(): void {
		successMessage('Exportación', 'Datos exportados correctamente');
		// TODO: Implementar lógica de exportación real
	}

	private handleError(message: string): void {
		this.error.set(true);
		this.errorMessage.set(message);
		this.loading.set(false);
		console.error(message);
	}
}
