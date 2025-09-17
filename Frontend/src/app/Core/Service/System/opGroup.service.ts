import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { OpGroupByAreaManagerMod, OpGroupMod, OpGroupOptionsMod } from '../../Models/System/OpGroupMod';
import { ZoneByBranchMod } from '../../Models/System/ZoneMod.model';
import { GenericService } from '../generic.service';

@Injectable({
	providedIn: 'root'
})
export class OpGroupService extends GenericService<OpGroupOptionsMod, OpGroupMod> {

	constructor(http: HttpClient) {
		const urlBase = environment.apiURL + 'api/OperatingGroup/';
		super(http, urlBase);
	}

	getByIdAreaManger(id: number | null): Observable<OpGroupByAreaManagerMod[]> {
		return this.http.get<OpGroupByAreaManagerMod[]>(`${this.baseUrl}GetByIdBranch/${id}`);
	}

}
