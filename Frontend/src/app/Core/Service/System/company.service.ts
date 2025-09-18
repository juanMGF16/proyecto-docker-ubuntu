<<<<<<< HEAD
import { HttpClient } from '@angular/common/http';
=======
>>>>>>> parent of 845d2803 (solucion de errores)
import { Injectable } from '@angular/core';
import { GenericService } from '../generic.service';
<<<<<<< HEAD
=======
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { CompanyConsultDTO, CompanyCreateDTO } from '../../Models/System/CompanyMod.model';
>>>>>>> parent of 845d2803 (solucion de errores)

@Injectable({
	providedIn: 'root'
})
export class CompanyService extends GenericService<CompanyCreateDTO, CompanyConsultDTO> {

	constructor(http: HttpClient) {
		const urlBase = environment.apiURL + 'api/Company/';
		super(http, urlBase);
	}

<<<<<<< HEAD
	createCompany(companyData: CompanyOptionsMod): Observable<CompanyMod> {
		return this.create(companyData);
	}

	partialUpdate(companyData: CompanyPartialUpdateMod): Observable<CompanyMod> {
		return this.http.patch<CompanyMod>(`${this.baseUrl}PartialUpdate/`, companyData);
=======
	// Método específico para registrar una empresa
	registerCompany(companyData: CompanyCreateDTO): Observable<CompanyConsultDTO> {
		return this.create(companyData);
	}

	// Método para obtener empresas por usuario (si lo necesitas más adelante)
	getCompaniesByUserId(userId: number): Observable<CompanyConsultDTO[]> {
		return this.http.get<CompanyConsultDTO[]>(`${this.baseUrl}GetByUserId/${userId}`);
>>>>>>> parent of 845d2803 (solucion de errores)
	}
}

