import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CategoriaEndpoints } from './endpoints';
import { ICategoriaModel } from '../interfaces/icategoria-model';

@Injectable({
  providedIn: 'root'
})
export class CategoriaService {
  constructor(private http: HttpClient) {}

  getCategorias(): Observable<ICategoriaModel[]> {
    return this.http.get<ICategoriaModel[]>(CategoriaEndpoints.GET_CATEGORIAS);
  }

  getCategoriaByNombre(nombre: string): Observable<ICategoriaModel> {
    const url = `${CategoriaEndpoints.GET_CATEGORIA_BY_NOMBRE}/${nombre}`;
    return this.http.get<ICategoriaModel>(url);
  }
}