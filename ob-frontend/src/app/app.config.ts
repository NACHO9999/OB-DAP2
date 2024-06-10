import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import  routes  from './app.routes';
import { AuthService } from './services/auth.service';
import { UsersService } from './services/user.service';
import { LoadingService } from './services/loading.service';
import { APIInterceptor } from './interceptors/api.interceptor';
import { LoadingInterceptor } from './interceptors/auth.interceptor';
import { ResponseInterceptor } from './interceptors/response.interceptor';
import { RoleGuard } from './guards/role.guard';
import { AuthGuard } from './guards/auth.guard';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';  // Import BrowserAnimationsModule
import { MatNativeDateModule } from '@angular/material/core';  // Import MatNativeDateModule
import { TokenInterceptor } from './interceptors/token.interceptor';
 
export const appConfig: ApplicationConfig = {
  
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    importProvidersFrom(BrowserModule, FormsModule, ReactiveFormsModule, HttpClientModule, MatNativeDateModule, BrowserAnimationsModule),
    provideHttpClient(),
    AuthService,
    UsersService,
    LoadingService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: APIInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
    AuthService,

    AuthGuard,
    RoleGuard,
  ]

};
