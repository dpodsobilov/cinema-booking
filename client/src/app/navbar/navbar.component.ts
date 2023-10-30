import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})

export class NavbarComponent {
  isAuthenticated:boolean = false;
  login: string ='';

  // constructor(private userService: UserService, @Inject('BASE_API_URL') private baseUrl: string) {
  //   this.updateUserInfo();
  //   this.userService.authChanged.subscribe(() => this.updateUserInfo());
  // }

  // updateUserInfo() {
  //   this.isAuthenticated = this.userService.isAuthenticated();
  //   if (this.isAuthenticated) {
  //     const userInfo = this.userService.getUserInfo();
  //     this.login = userInfo.login;
  //     this.photo = this.baseUrl + '/img/' + userInfo.photo;
  //   }
  // }

  // logout() {
  //   this.userService.logout();
  // }
}
