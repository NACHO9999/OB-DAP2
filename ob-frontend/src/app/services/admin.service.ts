import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminEndpoints } from './endpoints';
import { ICategoriaModel } from '../interfaces/icategoria-model';
import { IInvitacionModel } from '../interfaces/iinvitacion-model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  constructor(private http: HttpClient) {}

  getAdminByEmail(email: string): Observable<any> {
    const url = `${AdminEndpoints.GET_ADMIN_BY_EMAIL}/${email}`;
    return this.http.get<any>(url);
  }

  invitar(invitacion: IInvitacionModel): Observable<any> {
    const url = AdminEndpoints.INVITAR;
    return this.http.post(url, invitacion);
  }

  eliminarInvitacion(email: string): Observable<any> {
    const url = `${AdminEndpoints.ELIMINAR_INVITACION}/${email}`;
    return this.http.delete(url);
  }

  altaCategoria(categoria: ICategoriaModel): Observable<any> {
    const url = AdminEndpoints.ALTA_CATEGORIA;
    return this.http.post(url, categoria);
  }
}