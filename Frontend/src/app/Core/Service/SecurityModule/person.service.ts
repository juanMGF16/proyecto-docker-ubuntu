import { Injectable } from '@angular/core';
import { GenericService } from '../generic.service';
import { PersonAvailableMod, PersonMod } from '../../Models/SecurityModule/PersonMod.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PersonService extends GenericService<PersonMod, PersonMod> {
  constructor(http:HttpClient){
    const baseURL = environment.apiURL + 'api/Person/'
    super(http, baseURL);
  }

  getAvailable(): Observable<PersonAvailableMod[]>{
    return this.http.get<PersonAvailableMod[]>(`${this.baseUrl}GetAvailable/`);
  }

  getAllJWT(): Observable<PersonMod[]>{
    return this.http.get<PersonMod[]>(`${this.baseUrl}GetAllJWT/`);
  }
}
