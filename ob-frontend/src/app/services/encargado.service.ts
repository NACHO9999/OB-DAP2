import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IEncargadoModel } from '../interfaces/iencargado-model';
import { IUserCreate } from '../interfaces/user-create';
import { ISolicitudModel } from '../interfaces/isolicitud-model';
import { EncargadoEndpoints } from './endpoints';
import { IDuenoModel } from '../interfaces/idueno-model';

@Injectable({
  providedIn: 'root'
})
export class EncargadoService {
  constructor(private http: HttpClient) {}

  getEncargados(): Observable<IEncargadoModel[]> {
    return this.http.get<IEncargadoModel[]>(EncargadoEndpoints.GET_ENCARGADOS);
  }

  getEncargadoByEmail(email: string): Observable<IEncargadoModel> {
    return this.http.get<IEncargadoModel>(`${EncargadoEndpoints.GET_ENCARGADO_BY_EMAIL}/${email}`);
  }

  crearMantenimiento(mantenimiento: IUserCreate): Observable<any> {
    return this.http.post(EncargadoEndpoints.CREAR_MANTENIMIENTO, mantenimiento);
  }

  crearSolicitud(solicitud: ISolicitudModel): Observable<any> {
    return this.http.post(EncargadoEndpoints.CREAR_SOLICITUD, solicitud);
  }

  asignarSolicitud(solicitudId: string, emailMantenimiento: string): Observable<any> {
    return this.http.put(`${EncargadoEndpoints.ASIGNAR_SOLICITUD}/${solicitudId}/${emailMantenimiento}`, {});
  }

  getSolicitudByEdificio(nombre: string, direccion: string): Observable<number[]> {
    return this.http.get<number[]>(`${EncargadoEndpoints.GET_SOLICITUD_BY_EDIFICIO}/${nombre}/${direccion}`);
  }

  getSolicitudByMantenimiento(emailMantenimiento: string): Observable<number[]> {
    return this.http.get<number[]>(`${EncargadoEndpoints.GET_SOLICITUD_BY_MANTENIMIENTO}/${emailMantenimiento}`);
  }

  getTiempoPromedioAtencion(email: string): Observable<number | string> {
    return this.http.get<number | string>(`${EncargadoEndpoints.GET_TIEMPO_PROMEDIO_ATENCION}/${email}`);
  }

  getDueno(email: string): Observable<IDuenoModel> {
    return this.http.get<any>(`${EncargadoEndpoints.GET_DUENO}/${email}`);
  }

  asignarDueno(numero: number, edNombre: string, edDireccion: string, emailDueno: string): Observable<any> {
    return this.http.put(`${EncargadoEndpoints.ASIGNAR_DUENO}/${numero}/${edNombre}/${edDireccion}/${emailDueno}`, {});
  }
}