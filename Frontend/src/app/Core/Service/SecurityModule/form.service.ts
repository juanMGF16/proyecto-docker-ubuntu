import { Injectable } from '@angular/core';
import { GenericService } from '../generic.service';
import { FormMod } from '../../Models/SecurityModule/FormMod.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FormService extends GenericService<FormMod, FormMod> {

  constructor(http: HttpClient) {
    const urlBase = environment.apiURL + 'api/Form/'
    super(http, urlBase)
  }

  getAllJWT(): Observable<FormMod[]>{
    return this.http.get<FormMod[]>(`${this.baseUrl}GetAllJWT/`);
  }
}
