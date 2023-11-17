import { Component, Inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent {
  isAuthenticated: boolean = false;
  login: string = '';
  isAdmin: number = 0;
  layout: string = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {
    this.layout = localStorage.getItem('layout')!;
    this.updateUserInfo();
    this.authService.authChanged.subscribe(() => this.updateUserInfo());
  }

  updateUserInfo() {
    this.isAuthenticated = this.authService.isAuthenticated();
    if (this.isAuthenticated) {
      this.isAdmin = JSON.parse(localStorage.getItem('user')!).info.role;
    }
  }

  logout() {
    this.authService.logout();
  }

  goUser() {
    localStorage.setItem('layout', 'user');
    this.router.navigate(['/']);
  }
  goAdmin() {
    localStorage.setItem('layout', 'admin');
    this.router.navigate(['/admin']);
  }
}
