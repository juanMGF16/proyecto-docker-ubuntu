import { Injectable } from '@angular/core';
import { GenericService } from '../generic.service';
import { UserRoleMod, UserRoleOptionsMod } from '../../Models/SecurityModule/UserRoleMod.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserRoleService extends GenericService<UserRoleOptionsMod, UserRoleMod> {

  constructor(http: HttpClient) {
    const urlBase = environment.apiURL + 'api/UserRole/';
    super(http, urlBase);
  }

  getAllJWT(): Observable<UserRoleMod[]>{
    return this.http.get<UserRoleMod[]>(`${this.baseUrl}GetAllJWT/`);
  }
}
