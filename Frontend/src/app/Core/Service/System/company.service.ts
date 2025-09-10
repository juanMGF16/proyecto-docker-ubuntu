import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { CompanyMod, CompanyOptionsMod, CompanyPartialUpdateMod } from '../../Models/System/CompanyMod.model';
import { GenericService } from '../generic.service';
import { DashboardModel } from '../../Models/System/Others/Dashboard.model';

@Injectable({
	providedIn: 'root'
})
export class CompanyService extends GenericService<CompanyOptionsMod, CompanyMod> {

	constructor(http: HttpClient) {
		const urlBase = environment.apiURL + 'api/Company/';
		super(http, urlBase);
	}

	registerCompany(companyData: CompanyOptionsMod): Observable<CompanyMod> {
		return this.create(companyData);
	}

	partialUpdate(companyData: CompanyPartialUpdateMod): Observable<CompanyMod> {
		return this.http.patch<CompanyMod>(`${this.baseUrl}PartialUpdate/`, companyData);
	}

	getDashboard(companyId: number, branchId?: number, zoneId?: number): Observable<DashboardModel> {
		let params = new HttpParams().set('companyId', companyId);

		if (branchId !== undefined && branchId !== null) {
			params = params.set('branchId', branchId);
		}
		if (zoneId !== undefined && zoneId !== null) {
			params = params.set('zoneId', zoneId);
		}

		return this.http.get<DashboardModel>(environment.apiURL + 'api/Dashboard', { params });
	}
}

