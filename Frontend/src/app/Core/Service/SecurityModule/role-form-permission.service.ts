import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { GenericService } from '../generic.service';
import { RoleFormPermissionOptionsMod, RoleFormPermissionMod } from '../../Models/SecurityModule/RoleFormPermissionMod.model';

@Injectable({
  providedIn: 'root'
})
export class RoleFormPermissionService extends GenericService<RoleFormPermissionOptionsMod, RoleFormPermissionMod> {

  constructor(http: HttpClient) {
    const urlBase = environment.apiURL + 'api/RoleFormPermission/';
    super(http, urlBase);
  }
  getAllJWT(): Observable<RoleFormPermissionMod[]>{
    return this.http.get<RoleFormPermissionMod[]>(`${this.baseUrl}GetAllJWT/`);
  }
}
