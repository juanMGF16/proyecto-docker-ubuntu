import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export abstract class GenericService<TWrite, TRead> {

  constructor(
    protected http: HttpClient,
    protected baseUrl: string
  ) { }

  getAll(): Observable<TRead[]> {
    return this.http.get<TRead[]>(`${this.baseUrl}GetAll/`);
  }

  getById(id: number): Observable<TRead> {
    return this.http.get<TRead>(`${this.baseUrl}GetById/${id}`);
  }

  create(item: TWrite): Observable<TRead> {
    return this.http.post<TRead>(`${this.baseUrl}Create/`, item);
  }

  // Solo objeto
  update(item: TWrite): Observable<TRead> {
    return this.http.put<TRead>(`${this.baseUrl}Update/`, item);
  }

  // Objeto y Id
  // update(id: number, item: T): Observable<T> {
  //   return this.http.put<T>(`${this.baseUrl}/${id}`, item);
  // }

    delete(id: number, strategy = 0): Observable<any> {
    return this.http.delete(`${this.baseUrl}Delete/${id}/?strategy=${strategy}`);
  }
}