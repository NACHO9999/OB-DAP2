import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConstructoraEndpoints } from './endpoints';
import { IConstructoraModel } from '../interfaces/iconstructora-model';

@Injectable({
  providedIn: 'root'
})
export class ConstructoraService {
  constructor(private http: HttpClient) {}

  getConstructoras(): Observable<IConstructoraModel[]> {
    return this.http.get<IConstructoraModel[]>(ConstructoraEndpoints.GET_CONSTRUCTORAS);
  }

  getConstructoraByNombre(nombre: string): Observable<IConstructoraModel> {
    const url = `${ConstructoraEndpoints.GET_CONSTRUCTORA_BY_NOMBRE}/${nombre}`;
    return this.http.get<IConstructoraModel>(url);
  }

  insertConstructora(newConstructora: IConstructoraModel): Observable<any> {
    return this.http.post(ConstructoraEndpoints.INSERT_CONSTRUCTORA, newConstructora);
  }
}