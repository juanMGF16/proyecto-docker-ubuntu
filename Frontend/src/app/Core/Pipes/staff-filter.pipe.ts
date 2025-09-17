import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
	name: 'staffFilter',
	standalone: true
})
export class StaffFilterPipe implements PipeTransform {
	transform(items: any[], searchText: string): any[] {
		if (!items || !searchText) {
			return items;
		}

		const normalizedSearch = this.normalizeText(searchText.toLowerCase());

		return items.filter(item =>
			Object.values(item).some(value =>
				value &&
				this.normalizeText(value.toString().toLowerCase()).includes(normalizedSearch)
			)
		);
	}

	// Quitar tildes y normalizar
	private normalizeText(text: string): string {
		return text.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
	}
}
