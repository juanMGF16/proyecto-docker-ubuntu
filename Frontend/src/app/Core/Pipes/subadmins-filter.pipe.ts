import { Pipe, PipeTransform } from '@angular/core';
import { SubAdmin } from '../../Views/System/Admin/admin-subadmins/admin-subadmins.component';

@Pipe({
	name: 'subadminFilter',
	standalone: true
})
export class SubadminFilterPipe implements PipeTransform {
	transform(subadmins: SubAdmin[], searchText: string): SubAdmin[] {
		if (!subadmins || !searchText) {
			return subadmins;
		}

		// Normalizar el texto de búsqueda (quitar tildes y convertir a minúsculas)
		const normalizedSearch = this.normalizeText(searchText.toLowerCase());

		return subadmins.filter(subadmin =>
			this.normalizeText(subadmin.name.toLowerCase()).includes(normalizedSearch) ||
			this.normalizeText(subadmin.lastName.toLowerCase()).includes(normalizedSearch) ||
			this.normalizeText(subadmin.documentNumber.toLowerCase()).includes(normalizedSearch)
		);
	}

	// Método para normalizar texto (quitar tildes)
	private normalizeText(text: string): string {
		return text.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
	}
}
