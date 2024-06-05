import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IDuenoModel } from '../interfaces/idueno-model';
import { DuenoEndpoints } from './endpoints';

@Injectable({
  providedIn: 'root'
})
export class DuenoService {
  constructor(private http: HttpClient) {}

  getDuenoByEmail(email: string): Observable<IDuenoModel> {
    const url = `${DuenoEndpoints.GET_DUENO_BY_EMAIL}/${email}`;
    return this.http.get<IDuenoModel>(url);
  }

  insertDueno(newDueno: IDuenoModel): Observable<any> {
    const url = DuenoEndpoints.INSERT_DUENO;
    return this.http.post(url, newDueno);
  }
}