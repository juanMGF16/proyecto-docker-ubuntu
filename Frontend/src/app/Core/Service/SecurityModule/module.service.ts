import { Injectable } from '@angular/core';
import { GenericService } from '../generic.service';
import { ModuleMod } from '../../Models/SecurityModule/ModuleMod.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModuleService extends GenericService<ModuleMod, ModuleMod> {

  constructor(http: HttpClient) {
    const urlBase = environment.apiURL + 'api/Module/'
    super(http, urlBase)
  }

  getAllJWT(): Observable<ModuleMod[]>{
    return this.http.get<ModuleMod[]>(`${this.baseUrl}GetAllJWT/`);
  }
}
