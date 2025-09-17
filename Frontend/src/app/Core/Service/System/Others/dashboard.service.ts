import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GenericService } from '../../generic.service';
import { environment } from '../../../../../environments/environment';
import { DashboardBranchModel, DashboardModel, ZoneDashboard } from '../../../Models/System/Others/Dashboard.model';

@Injectable({
	providedIn: 'root'
})
export class DashboardService extends GenericService<any, any> {

	constructor(http: HttpClient) {
		const urlBase = environment.apiURL + 'api/Dashboard/';
		super(http, urlBase);
	}

	getDashboardCompany(companyId: number, branchId?: number, zoneId?: number): Observable<DashboardModel> {
		let params = new HttpParams().set('companyId', companyId);

		if (branchId !== undefined && branchId !== null) {
			params = params.set('branchId', branchId);
		}
		if (zoneId !== undefined && zoneId !== null) {
			params = params.set('zoneId', zoneId);
		}

		return this.http.get<DashboardModel>(this.baseUrl, { params });
	}

	getDashboardBranch(branchId: number): Observable<DashboardBranchModel> {
		return this.http.get<DashboardBranchModel>(this.baseUrl + `branch/${branchId}`);
	}

	getDashboardZone(zoneId: number): Observable<ZoneDashboard> {
		return this.http.get<ZoneDashboard>(this.baseUrl + `zone/${zoneId}`);
	}
}
