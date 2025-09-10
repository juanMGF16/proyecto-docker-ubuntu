import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { UserHasCompanyMod, UserMod, UserOptionsMod, UserPartialUpdateMod } from '../../Models/SecurityModule/UserMod.model';
import { GenericService } from '../generic.service';

@Injectable({
	providedIn: 'root'
})
export class UserService extends GenericService<UserOptionsMod, UserMod> {

	constructor(http: HttpClient) {
		const urlBase = environment.apiURL + 'api/User/';
		super(http, urlBase);
	}

	getAllJWT(): Observable<UserMod[]> {
		return this.http.get<UserMod[]>(`${this.baseUrl}GetAllJWT/`);
	}

	// Ahora devuelve el DTO con hasCompany y companyId
	hasCompany(): Observable<UserHasCompanyMod> {
		return this.http.get<UserHasCompanyMod>(`${this.baseUrl}HasCompany/`);
	}

	partialUpdate(userData: UserPartialUpdateMod): Observable<UserMod> {
		return this.http.patch<UserMod>(`${this.baseUrl}PartialUpdate/`, userData);
	}

	changePassword(currentPassword: string, newPassword: string): Observable<void> {
		return this.http.post<void>(`${this.baseUrl}ChangePassword/`, {
			currentPassword,
			newPassword
		});
	}
}
