import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { UsersService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  resultMessage = '';

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
  }

  ngOnInit(): void {}

  login() {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      this._userService.login(email, password).subscribe(
        (data) => {
          this._authService.setToken(data.token);
          this._authService.setUserRole(data.role);
          this.saveUserInfo(JSON.stringify({ email }));
          let urlToGo;
          this._route.queryParams.subscribe((params) => {
            if (params['returnUrl']) {
              urlToGo = params['returnUrl'];
            }
          });
          if (urlToGo) {
            this._router.navigate([urlToGo]);
          } else {
            this._router.navigate(['/']);
          }
        },
        (error) => {
          console.error('Login error', error);
          this.resultMessage = 'Login failed. Please try again.';
        }
      );
    } else {
      this.resultMessage = 'Invalid email or password format.';
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

  private saveUserInfo(userInfo: string): void {
    localStorage.setItem('userInfo', userInfo);
  }
}