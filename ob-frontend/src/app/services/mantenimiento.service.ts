import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MantenimientoEndpoints } from './endpoints';
import { ISolicitudModel } from '../interfaces/isolicitud-model';

@Injectable({
  providedIn: 'root'
})
export class MantenimientoService {

  constructor(private http: HttpClient) { }

  atenderSolicitud(solicitudId: string): Observable<any> {
    return this.http.put<any>(`${MantenimientoEndpoints.ATENDER_SOLICITUD}/${solicitudId}`, {});
  }

  completarSolicitud(solicitudId: string): Observable<any> {
    return this.http.put<any>(`${MantenimientoEndpoints.COMPLETAR_SOLICITUD}/${solicitudId}`, {});
  }

  getSolicitudesParaAtender(): Observable<ISolicitudModel[]> {
    return this.http.get<ISolicitudModel[]>(`${MantenimientoEndpoints.GET_SOLICITUDES}`);
  }

  getSolicitudesAtendiendo(): Observable<ISolicitudModel[]> {
    return this.http.get<ISolicitudModel[]>(`${MantenimientoEndpoints.GET_SOLICITUDES_ATENDIENDO}`);
  }
}