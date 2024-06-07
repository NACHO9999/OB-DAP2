import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute, NavigationEnd, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { UsersService } from './services/user.service';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterOutlet],
  providers: [UsersService, AuthService, HttpClient],
  
})
export class AppComponent implements OnInit {
  loginForm: FormGroup;
  resultMessage = '';
  hideAppContent = false;
  title = 'Gestion de Edificios'

  constructor(
    private fb: FormBuilder,
    private _userService: UsersService,
    private _authService: AuthService,
    private _route: ActivatedRoute,
    private _router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });

    this._router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.hideAppContent = event.url !== '/';
        if (event.url === '/' && this.isLoggedIn()) {
          const role = this._authService.getUserRole();
          this._router.navigate(['/' + role]);
        }
      }
    });
  }

  ngOnInit(): void {
    if (this.isLoggedIn() && this._router.url === '/') {
      const role = this._authService.getUserRole();
      this._router.navigate(['/' + role]);
    }
  }

  login() {
    if (!this.isLoggedIn()) {
      if (this.loginForm.valid) {
        const { email, password } = this.loginForm.value;
        this._userService.login(email, password).subscribe(
          (data: { token: string; role: string; }) => {
            this._authService.setToken(data.token);
            this._authService.setUserRole(data.role);
            this.saveUserInfo(JSON.stringify({ email }));
            this._router.navigate(['/' + data.role]);
          },
          (error: string) => {
            console.error('Login error', error);
            this.resultMessage = 'Login failed. Please try again.';
          }
        );
      } else {
        this.resultMessage = 'Invalid email or password format.';
      }
    } else {
      this._router.navigate(['/' + this._authService.getUserRole()]);
    }
  }

  isLoggedIn(): boolean {
    return this._authService.isAuthenticated();
  }

  logout() {
    this._authService.removeToken();
    this._authService.removeUserRole();
    localStorage.removeItem('userInfo');
  }

  goToInvitaciones() {
    this._router.navigate(['/invitaciones']);
  }

  private saveUserInfo(userInfo: string): void {
    localStorage.setItem('userInfo', userInfo);
  }
}
