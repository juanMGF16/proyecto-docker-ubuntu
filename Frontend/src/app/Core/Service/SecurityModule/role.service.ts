import { Injectable } from '@angular/core';
import { GenericService } from '../generic.service';
import { RoleMod } from '../../Models/SecurityModule/RoleMod.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleService extends GenericService<RoleMod, RoleMod> {

  constructor(http: HttpClient) {
    const baseURL = environment.apiURL + 'api/Role/';
    super(http, baseURL);
  }

    getAllJWT(): Observable<RoleMod[]>{
    return this.http.get<RoleMod[]>(`${this.baseUrl}GetAllJWT/`);
  }
}
