import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IUserService, Session } from '../interfaces/iuser-service';
import { UsersEndpoints } from './endpoints';


@Injectable({
  providedIn: 'root',
})
export class UsersService implements IUserService {
  constructor(private _httpService: HttpClient) {}

  login(email: string, password: string): Observable<any> {
    return this._httpService.post<Session>(`${UsersEndpoints.LOGIN}`, {
      "Email" : email,
      "Contrasena" : password
    });
  }

  logout(): Observable<any> {
    return this._httpService.post(`${UsersEndpoints.LOGOUT}`, {});
  }
}