import { Pipe, PipeTransform } from '@angular/core';
import { InventoryItemTable } from '../../Components/System/Area_Manager/Inventory/inventory-table/inventory-table.component';

@Pipe({
	name: 'inventoryBaseFilter',
	standalone: true
})
export class InventoryBaseFilterPipe implements PipeTransform {

	transform(items: InventoryItemTable[], searchText: string, categoryFilter: string, statusFilter: string): InventoryItemTable[] {
		if (!items) return [];

		let filteredItems = items;

		// Filtro por búsqueda de texto (nombre, código o descripción)
		if (searchText) {
			const normalizedSearch = this.normalizeText(searchText.toLowerCase());
			filteredItems = filteredItems.filter(item =>
				this.normalizeText(item.name.toLowerCase()).includes(normalizedSearch) ||
				this.normalizeText(item.code.toLowerCase()).includes(normalizedSearch) ||
				(item.description && this.normalizeText(item.description.toLowerCase()).includes(normalizedSearch))
			);
		}

		// Filtro por categoría
		if (categoryFilter && categoryFilter !== 'all') {
			filteredItems = filteredItems.filter(item => item.category === categoryFilter);
		}

		// Filtro por estado
		if (statusFilter && statusFilter !== 'all') {
			filteredItems = filteredItems.filter(item => item.state === statusFilter);
		}

		return filteredItems;
	}

	// Método para normalizar texto (quitar tildes)
	private normalizeText(text: string): string {
		return text.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
	}
}
