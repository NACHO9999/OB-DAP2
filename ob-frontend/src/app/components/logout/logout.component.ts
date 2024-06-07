import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UsersService } from '../../services/user.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css'],
  standalone: true,
})
export class LogoutButtonComponent {
  constructor(
    private authService: AuthService,
    private usersService: UsersService,
    private router: Router
  ) { }

  logout() {

    this.usersService.logout();
    this.authService.removeToken();
    this.authService.removeUserRole();
    localStorage.removeItem('userInfo');
    this.router.navigate(['/']);
  }
}
