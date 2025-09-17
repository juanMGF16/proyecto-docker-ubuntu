import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
	name: 'categoryInventoryBase',
	standalone: true
})
export class CategoryInventoryBasePipe implements PipeTransform {

	transform(category: string): { icon: string; label: string } {
		if (category === 'all') {
			return {
				icon: 'all_inclusive',
				label: 'Todas'
			};
		}

		const categoryMap: Record<string, { icon: string; label: string }> = {
			technology: { icon: 'devices', label: 'Tecnología' },
			furniture: { icon: 'chair', label: 'Mobiliario' },
			supplies: { icon: 'description', label: 'Suministros' },
			tools: { icon: 'build', label: 'Herramientas' },
			equipment: { icon: 'settings', label: 'Equipos' }
		};

		return categoryMap[category] || {
			icon: 'inventory_2',
			label: this.capitalizeFirstLetter(category)
		};
	}

	private capitalizeFirstLetter(text: string): string {
		return text.charAt(0).toUpperCase() + text.slice(1);
	}
}
