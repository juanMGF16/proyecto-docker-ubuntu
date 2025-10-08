// ===== SERVICIO DE AUTENTICACIÓN =====
// Gestiona el flujo de autenticación del usuario: login, registro, recuperación de contraseñas,
// validación de tokens JWT y extracción de información del payload. Además, maneja la
// persistencia del token en localStorage y provee utilidades para validar roles y sesiones.

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

	// Inicia sesión y obtiene un JWT
	login(credentials: { username: string; password: string }) {
		return this.http.post<{ token: string }>(`${this.baseUrl}Login`, credentials);
	}

	// Registra un nuevo usuario en el sistema
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

	// Obtiene todos los roles disponibles desde el backend
	getAllRoles(): Observable<RoleMod[]> {
		return this.http.get<RoleMod[]>(`${this.baseUrl}GetAllRoles/`);
	}

	// Envía solicitud para recuperar contraseña
	forgotPassword(email: string): Observable<any> {
		return this.http.post<any>(`${this.baseUrl}forgot-password`, { email });
	}

	// Valida un token de recuperación recibido
	validateRecoveryToken(token: string): Observable<any> {
		return this.http.get<any>(`${this.baseUrl}validate-recovery-token?token=${token}`);
	}

	// Restablece la contraseña con un nuevo valor
	resetPassword(data: { token: string; newPassword: string; }): Observable<any> {
		return this.http.post<any>(`${this.baseUrl}reset-password`, data);
	}

	// Obtiene el token almacenado en localStorage
	getToken(): string | null {
		return localStorage.getItem(this.tokenKey);
	}

	// Guarda el token en localStorage
	saveToken(token: string): void {
		localStorage.setItem(this.tokenKey, token);
	}

	// Verifica si el token existe y sigue siendo válido
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

	// Decodifica y retorna el payload del token
	getTokenPayload(): JwtPayload {
		const token = this.getToken();
		return token ? jwtDecode<JwtPayload>(token) : { nameid: '', personId: '', unique_name: '', role: '', exp: 0 };
	}

	// Obtiene el ID del usuario desde el token
	getIdUser(): string {
		return this.getTokenPayload().nameid;
	}

	// Obtiene el ID de la persona desde el token
	getIdPerson(): string {
		return this.getTokenPayload().personId;
	}

	// Obtiene el ID de la persona desde el token
	getRole(): string {
		return this.getTokenPayload().role;
	}

	// Obtiene el username desde el token
	getUsername(): string {
		return this.getTokenPayload().unique_name;
	}
}
