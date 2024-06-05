import { Observable } from "rxjs/internal/Observable"

export interface Session {
    token: string
}
  
export interface IUserService {
    login(email: string, password: string): Observable<Session>
}
  