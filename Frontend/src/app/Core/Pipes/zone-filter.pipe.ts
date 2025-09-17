import { Pipe, PipeTransform } from '@angular/core';
import { BranchDetailsMod } from '../Models/System/BranchMod.model';


@Pipe({
  name: 'zoneFilter',
  standalone: true
})
export class ZoneFilterPipe implements PipeTransform {
  transform(zones: BranchDetailsMod['zones'] | undefined, searchText: string): BranchDetailsMod['zones'] {
    if (!zones) return [];
    if (!searchText) return zones;

    // Normalizar el texto de búsqueda (quitar tildes y convertir a minúsculas)
    const normalizedSearch = this.normalizeText(searchText.toLowerCase());

    return zones.filter(zone =>
      this.normalizeText(zone.name.toLowerCase()).includes(normalizedSearch) ||
      this.normalizeText(zone.inChargeFullName.toLowerCase()).includes(normalizedSearch) ||
      zone.itemsCount.toString().includes(normalizedSearch) // permite buscar por número de ítems
    );
  }

  // Método para normalizar texto (quitar tildes)
  private normalizeText(text: string): string {
    return text.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
  }
}
