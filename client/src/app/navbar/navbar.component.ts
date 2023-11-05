import { Component, Inject } from '@angular/core';
import { AuthService, UserInfo } from '../services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent {
  isAuthenticated: boolean = false;
  login: string = '';

  constructor(
    private authService: AuthService,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {
    this.updateUserInfo();
    this.authService.authChanged.subscribe(() => this.updateUserInfo());
  }

  updateUserInfo() {
    this.isAuthenticated = this.authService.isAuthenticated();
  }

  logout() {
    this.authService.logout();
  }
}
