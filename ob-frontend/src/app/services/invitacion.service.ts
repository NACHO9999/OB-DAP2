import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InvitacionEndpoints } from './endpoints';

@Injectable({
  providedIn: 'root'
})
export class InvitacionService {

  constructor(private http: HttpClient) { }

  invitacionAccepted(email: string, contrasena: string): Observable<any> {
    return this.http.post<any>(`${InvitacionEndpoints.INVITACION_ACCEPTED}/${email}/${contrasena}`, {});
  }
}