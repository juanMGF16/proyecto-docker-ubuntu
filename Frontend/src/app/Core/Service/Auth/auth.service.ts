import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { RoleMod } from '../../Models/SecurityModule/RoleMod.model';

export interface JwtPayload {
	nameid: string;
	personId: string;
	unique_name: string;
	role: string;
	exp: number;
}

@Injectable({
	providedIn: 'root'
})
export class AuthService {

	private readonly tokenKey = 'auth_token';
	private readonly baseUrl = `${environment.apiURL}api/Auth/`;

	constructor(private http: HttpClient, private router: Router) { }

	login(credentials: { username: string; password: string }) {
		return this.http.post<{ token: string }>(`${this.baseUrl}Login`, credentials);
	}

	register(userData: {
		username: string;
		password: string;
		name: string;
		lastName: string;
		email: string;
		documentType: string;
		documentNumber: string;
		phone: string;
	}): Observable<any> {
		return this.http.post<any>(`${this.baseUrl}Register`, userData);
	}

	getAllRoles(): Observable<RoleMod[]> {
		return this.http.get<RoleMod[]>(`${this.baseUrl}GetAllRoles/`);
	}

	logout(): void {
		localStorage.removeItem(this.tokenKey);
		this.router.navigate(['']);
	}

	getToken(): string | null {
		return localStorage.getItem(this.tokenKey);
	}

	saveToken(token: string): void {
		localStorage.setItem(this.tokenKey, token);
	}

	isAuthenticated(): boolean {
		const token = this.getToken();
		if (!token) return false;

		const { exp } = this.getTokenPayload();
		return exp * 1000 > Date.now(); // token aún válido
	}

	// SIN USO!! - Funcion usada en token-monitor-service.ts
	refreshToken() {
		return this.http.post<{ token: string }>(
			`${this.baseUrl}Refresh`,
			null
		);
	}

	getTokenPayload(): JwtPayload {
		const token = this.getToken();
		return token ? jwtDecode<JwtPayload>(token) : { nameid: '', personId: '', unique_name: '', role: '', exp: 0 };
	}

	getIdUser(): string {
		return this.getTokenPayload().nameid;
	}

	getIdPerson(): string {
		return this.getTokenPayload().personId;
	}

	getRole(): string {
		return this.getTokenPayload().role;
	}

	getUsername(): string {
		return this.getTokenPayload().unique_name;
	}
}
