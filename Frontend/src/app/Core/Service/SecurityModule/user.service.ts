import { Injectable } from '@angular/core';
import { GenericService } from '../generic.service';
import { UserMod, UserOptionsMod, UserPartialUpdate } from '../../Models/SecurityModule/UserMod.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

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

	hasCompany(): Observable<boolean> {
		return this.http.get<boolean>(`${this.baseUrl}HasCompany/`);
	}

	partialUpdate(userData: UserPartialUpdate): Observable<UserMod> {
		return this.http.patch<UserMod>(`${this.baseUrl}PartialUpdate/`, userData);
	}

	changePassword(currentPassword: string, newPassword: string): Observable<void> {
		return this.http.post<void>(`${this.baseUrl}ChangePassword/`, {
			currentPassword,
			newPassword
		});
	}
}
