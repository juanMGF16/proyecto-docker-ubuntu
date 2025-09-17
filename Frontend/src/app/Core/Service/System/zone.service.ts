import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';


import { catchError, map, Observable, throwError } from 'rxjs';
import { ApiResponse } from '../../Models/System/Others/BranchNestedCreation.model';
import { ZoneCreateRequestDTO } from '../../Models/System/Others/ZoneNestedCreation.model';
import { ZoneByBranchMod, ZoneDetailsApi, ZoneDetailsMod, ZoneInChargesMod, ZoneMod, ZoneOptionsMod, ZonePartialUpdateMod } from '../../Models/System/ZoneMod.model';
import { GenericService } from '../generic.service';
import { CATEGORY_MAP, STATE_MAP } from '../../Constants/item-mappings';
import { ZONE_STATE_MAP } from '../../Constants/zone-mapping';

@Injectable({
	providedIn: 'root'
})
export class ZoneService extends GenericService<ZoneOptionsMod, ZoneMod> {

	constructor(http: HttpClient) {
		const urlBase = environment.apiURL + 'api/Zone/';
		super(http, urlBase);
	}


	getByIdBranch(id: number | null): Observable<ZoneByBranchMod[]> {
		return this.http.get<ZoneByBranchMod[]>(`${this.baseUrl}GetByIdBranch/${id}`);
	}

	getZoneDetailsById(id: number): Observable<ZoneDetailsMod> {
		return this.http.get<ZoneDetailsApi>(`${this.baseUrl}GetZoneDetailsById/${id}`).pipe(
			map(apiZone => ({
				...apiZone,
				state: ZONE_STATE_MAP[apiZone.state] || apiZone.state,
				items: apiZone.items.map(item => ({
					itemId: item.itemId,
					code: item.code,
					name: item.name,
					description: item.description,
					category: CATEGORY_MAP[item.categoryId] || 'Desconocido',
					state: STATE_MAP[item.stateId] || 'Desconocido'
				}))
			})),
			catchError(error => {
				console.error(`Error cargando zona ${id}:`, error);
				return throwError(() => new Error('No se pudo cargar la información de la zona.'));
			})
		);
	}

	getInCharges(id: number | null): Observable<ZoneInChargesMod[]> {
		return this.http.get<ZoneInChargesMod[]>(`${this.baseUrl}GetInCharges/${id}`);
	}

	getByIdAreaManager(id: number): Observable<ZoneMod> {
		return this.http.get<ZoneMod>(`${this.baseUrl}GetZoneByAreaManager/${id}`);
	}

	createWithEncZone(request: ZoneCreateRequestDTO): Observable<ApiResponse<ZoneMod>> {
		return this.http.post<ApiResponse<ZoneMod>>(
			`${environment.apiURL}api/ZoneRegistration/Create-With-EncZone`,
			request
		);
	}

	partialUpdate(branchData: ZonePartialUpdateMod): Observable<ZoneMod> {
		return this.http.patch<ZoneMod>(`${this.baseUrl}PartialUpdate/`, branchData);
	}
}
