import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor{
  constructor(private authService: AuthService) {}
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();
    console.log('TokenInterceptor: ', token);
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: token
          
        }
        
      });
      console.log('TokenInterceptor: ', token);
    }
    return next.handle(request);
  }
}