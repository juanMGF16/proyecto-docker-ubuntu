import { Pipe, PipeTransform } from '@angular/core';
import { Zone } from '../../Views/System/Admin/admin-branch/admin-branch.component';

@Pipe({
	name: 'zoneFilter',
	standalone: true
})
export class ZoneFilterPipe implements PipeTransform {
	transform(zones: Zone[], searchText: string): Zone[] {
		if (!zones || !searchText) {
			return zones;
		}

		// Normalizar el texto de búsqueda (quitar tildes y convertir a minúsculas)
		const normalizedSearch = this.normalizeText(searchText.toLowerCase());

		return zones.filter(zone =>
			this.normalizeText(zone.name.toLowerCase()).includes(normalizedSearch) ||
			this.normalizeText(zone.encargado.toLowerCase()).includes(normalizedSearch)
		);
	}

	// Método para normalizar texto (quitar tildes)
	private normalizeText(text: string): string {
		return text.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
	}
}
