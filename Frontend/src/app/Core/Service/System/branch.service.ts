import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';

import { BranchByCompanyMod, BranchDetailsMod, BranchInChargeMod, BranchInChargesMod, BranchMod, BranchOptionsMod, BranchPartialUpdateMod } from '../../Models/System/BranchMod.model';

import { Observable } from 'rxjs';
import { ApiResponse, BranchCreateRequestDTO } from '../../Models/System/Others/BranchNestedCreation.model';
import { GenericService } from '../generic.service';
import { DashboardBranchModel } from '../../Models/System/Others/Dashboard.model';

@Injectable({
	providedIn: 'root'
})
export class BranchService extends GenericService<BranchOptionsMod, BranchMod> {

	constructor(http: HttpClient) {
		const urlBase = environment.apiURL + 'api/Branch/';
		super(http, urlBase);
	}

	getByIdCompany(id: number | null): Observable<BranchByCompanyMod[]> {
		return this.http.get<BranchByCompanyMod[]>(`${this.baseUrl}GetByIdCompany/${id}`);
	}

	getByDetails(id: number): Observable<BranchDetailsMod> {
		return this.http.get<BranchDetailsMod>(`${this.baseUrl}GetBranchDetails/${id}`);
	}

	getInCharge(id: number): Observable<BranchInChargeMod> {
		return this.http.get<BranchInChargeMod>(`${this.baseUrl}GetInCharge/${id}`);
	}

	getInCharges(id: number | null): Observable<BranchInChargesMod[]> {
		return this.http.get<BranchInChargesMod[]>(`${this.baseUrl}GetInCharges/${id}`);
	}

	getByIdInCharge(id: number): Observable<BranchMod> {
		return this.http.get<BranchMod>(`${this.baseUrl}GetBranchByInCharge/${id}`);
	}

	getDashboard(branchId: number): Observable<DashboardBranchModel> {
		return this.http.get<DashboardBranchModel>(environment.apiURL + `api/Dashboard/branch/${branchId}`);
	}

	createWithAdmin(request: BranchCreateRequestDTO): Observable<ApiResponse<BranchMod>> {
		return this.http.post<ApiResponse<BranchMod>>(
			`${environment.apiURL}api/BranchRegistration/Create-With-Admin`,
			request
		);
	}

	partialUpdate(branchData: BranchPartialUpdateMod): Observable<BranchMod> {
		return this.http.patch<BranchMod>(`${this.baseUrl}PartialUpdate/`, branchData);
	}


}
