import { Injectable } from '@angular/core';
import { GenericService } from '../generic.service';
import { FormModuleMod, FormModuleOptionsMod } from '../../Models/SecurityModule/FormModuleMod.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FormModuleService extends GenericService<FormModuleOptionsMod, FormModuleMod> {

  constructor(http: HttpClient) {
    const urlBase = environment.apiURL + 'api/FormModule/';
    super(http, urlBase);
  }

  getAllJWT(): Observable<FormModuleMod[]>{
    return this.http.get<FormModuleMod[]>(`${this.baseUrl}GetAllJWT/`);
  }
}
