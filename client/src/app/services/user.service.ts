import { HttpClient } from '@angular/common/http';
import { EventEmitter, Inject, Injectable, Output } from '@angular/core';
import { Router } from '@angular/router';

export interface UserInfo {
  email: string;
  authData: string;
}

@Injectable({
  providedIn: 'root',
})
export class UserService {
  @Output() errors: EventEmitter<any> = new EventEmitter();
  @Output() authChanged: EventEmitter<any> = new EventEmitter();

  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
    private router: Router,
  ) {}

  register(
    email: string,
    password: string,
    firstName: string,
    lastName: string,
  ) {
    const formData = new FormData();

    formData.append('Email', email!);
    formData.append('Password', password!);
    formData.append('FirstName', firstName);
    formData.append('Lastname', lastName);

    this.http.post(this.baseUrl + '/register', formData).subscribe({
      next: (result) => {
        this.login(email, password, false);
        this.router.navigate(['']);
      },
      error: (e) => {
        this.errors.emit(e);
      },
    });
  }

  login(email: string, password: string, rememberMe: boolean) {
    const formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);

    const storage = rememberMe ? localStorage : sessionStorage;

    this.http.post(this.baseUrl + '/login', formData).subscribe({
      next: (user: any) => {
        const authData = window.btoa(email + ':' + password);
        const UserInfo: UserInfo = { email: email, authData: authData };
        storage.setItem('userInfo', JSON.stringify(UserInfo));
        storage.setItem('isAuthenticated', 'true');
        this.authChanged.emit();
        this.router.navigate(['']);
      },
      error: (e) => {
        this.errors.emit(e);
      },
    });
  }

  logout() {
    localStorage.removeItem('userInfo');
    localStorage.removeItem('isAuthenticated');

    sessionStorage.removeItem('userInfo');
    sessionStorage.removeItem('isAuthenticated');

    this.authChanged.emit();
    this.router.navigate(['']);
  }

  isAuthenticated(): boolean {
    const isLocal = localStorage.getItem('isAuthenticated') == 'true';
    const isSession = sessionStorage.getItem('isAuthenticated') == 'true';
    return isLocal || isSession;
  }

  getUserInfo(): UserInfo {
    let user = JSON.parse(localStorage.getItem('userInfo')!) as UserInfo;
    if (user == null) {
      user = JSON.parse(sessionStorage.getItem('userInfo')!) as UserInfo;
    }
    return user;
  }
}
