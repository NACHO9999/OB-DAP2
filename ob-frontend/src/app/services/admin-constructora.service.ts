import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminConstructoraEndpoints } from './endpoints';
import { IEdificioModel } from '../interfaces/iedificio-model'; 
import { IDeptoModel } from '../interfaces/idepto-model'; 
import { IConstructoraModel } from '../interfaces/iconstructora-model';

@Injectable({
  providedIn: 'root'
})
export class AdminConstructoraService {
  constructor(private http: HttpClient) {}

  borrarEdificio(nombre: string, direccion: string): Observable<any> {
    const url = `${AdminConstructoraEndpoints.BORRAR_EDIFICIO}/${nombre}/${direccion}`;
    console.log(url);
    return this.http.delete(url);
  }
  borrarDepto(numero: number, nombre: string, direccion: string): Observable<any> {
    const url = `${AdminConstructoraEndpoints.BORRAR_DEPTO}/${numero}/${nombre}/${direccion}`;
    return this.http.delete(url);
  }
  getEdificio(nombre: string, direccion: string): Observable<IEdificioModel> {
    const url = `${AdminConstructoraEndpoints.GET_EDIFICIO}/${nombre}/${direccion}`;
    return this.http.get<IEdificioModel>(url);
  }

  getEdificiosPorAdmin(): Observable<IEdificioModel[]> {
    const url = AdminConstructoraEndpoints.GET_EDIFICIOS_POR_ADMIN;
    return this.http.get<IEdificioModel[]>(url);
  }

  editarEdificio(edificio: IEdificioModel): Observable<any> {
    const url = AdminConstructoraEndpoints.EDITAR_EDIFICIO;
    return this.http.put(url, edificio);
  }

  crearEdificio(edificio: IEdificioModel): Observable<any> {
    const url = AdminConstructoraEndpoints.CREAR_EDIFICIO;
    return this.http.post(url, edificio);
  }

  crearDepto(depto: IDeptoModel): Observable<any> {
    const url = AdminConstructoraEndpoints.CREAR_DEPTO;
    return this.http.post(url, depto);
  }

  getDepto(numero: number, nombre: string, direccion: string): Observable<IDeptoModel> {
    const url = `${AdminConstructoraEndpoints.GET_DEPTO}/${numero}/${nombre}/${direccion}`;
    return this.http.get<IDeptoModel>(url);
  }

  editarDepto(depto: IDeptoModel): Observable<any> {
    const url = AdminConstructoraEndpoints.EDITAR_DEPTO;
    return this.http.put(url, depto);
  }

  getEdificiosConEncargados(): Observable<IEdificioModel[]> {
    const url = AdminConstructoraEndpoints.GET_EDIFICIOS_CON_ENCARGADOS;
    return this.http.get<IEdificioModel[]>(url);
  }

  getEdificiosSinEncargados(): Observable<IEdificioModel[]> {
    const url = AdminConstructoraEndpoints.GET_EDIFICIOS_SIN_ENCARGADOS;
    return this.http.get<IEdificioModel[]>(url);
  }

  editarConstructora(nombre: string): Observable<any> {
    const url = `${AdminConstructoraEndpoints.EDIT_CONSTRUCTORA}/${nombre}`;
    return this.http.put(url, { id: '3DA3FEFB-383D-4E54-9DF7-CBF9E3CF8F33', nombre: 'Constructora 1' });
}

  filtrarPorNombreEncargado(nombre: string): Observable<IEdificioModel[]> {
    const url = `${AdminConstructoraEndpoints.FILTRAR_POR_NOMBRE_ENCARGADO}/${nombre}`;
    return this.http.get<IEdificioModel[]>(url, {});
  }
  asignarEncargado(emailEncargado: string, edNombre: string, edDireccion: string): Observable<any> {
    const url = `${AdminConstructoraEndpoints.ASIGNAR_ENCARGADO}/${emailEncargado}/${edNombre}/${edDireccion}`;
    return this.http.put(url, {});
  }

  desasignarEncargado(edNombre: string, edDireccion: string): Observable<any> {
    const url = `${AdminConstructoraEndpoints.DESASIGNAR_ENCARGADO}/${edNombre}/${edDireccion}`;
    return this.http.put(url, {});
  }
  elegirConstructora(nombre: string): Observable<any> {
    const url = `${AdminConstructoraEndpoints.ELEGIR_CONSTRUCTORA}/${nombre}`;
    return this.http.put(url, {});
  }

  tieneConstructora(): Observable<boolean> {
    return this.http.get<boolean>(AdminConstructoraEndpoints.TIENE_CONSTRUCTORA);
  }
  getConstructoras(): Observable<IConstructoraModel[]> {
    return this.http.get<IConstructoraModel[]>(AdminConstructoraEndpoints.GET_CONSTRUCTORAS);
  }
  crearConstructora(nombre: string): Observable<any> {
    const url = `${AdminConstructoraEndpoints.CREAR_CONSTRUCTORA}/${nombre}`;
    return this.http.post(url, {});
  }
}