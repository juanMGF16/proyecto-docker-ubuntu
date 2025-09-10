import { Injectable } from '@angular/core';
import { GenericService } from '../generic.service';
import { PermissionMod } from '../../Models/SecurityModule/PermissionMod.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PermissionService extends GenericService<PermissionMod, PermissionMod> {

  constructor(http: HttpClient) {
    const urlBase = environment.apiURL + 'api/Permission/'
    super(http, urlBase)
  }

  getAllJWT(): Observable<PermissionMod[]>{
    return this.http.get<PermissionMod[]>(`${this.baseUrl}GetAllJWT/`);
  }
}
