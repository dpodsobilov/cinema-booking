import { EventEmitter, Inject, Injectable, Output } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

export interface UserInfo {
  email: string;
  authData: string;
  info: User;
}

export interface User {
  userId: number;
  role: number;
  name: string;
  surname: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  @Output() errors: EventEmitter<any> = new EventEmitter();
  @Output() authChanged: EventEmitter<any> = new EventEmitter();

  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
    private router: Router,
  ) {}

  register(email: string, password: string, name: string, surname: string) {
    const formData = new FormData();

    formData.append('Email', email!);
    formData.append('Password', password!);
    formData.append('Name', name!);
    formData.append('Surname', surname!);

    this.http.post(this.baseUrl + '/register', formData).subscribe({
      next: (result) => {
        this.login(email, password);
      },
      error: (e: HttpErrorResponse) => {
        this.errors.emit(e);
      },
    });
  }

  login(email: string, password: string) {
    const formData = new FormData();
    formData.append('email', email!);
    formData.append('password', password!);

    const storage = localStorage;

    this.http.post(this.baseUrl + '/login', formData).subscribe({
      next: (user: any) => {
        const AUTH_DATA = window.btoa(email + ':' + password);
        const USER_INFO: UserInfo = {
          email: email,
          authData: AUTH_DATA,
          info: {
            userId: user.userId,
            role: user.role,
            name: user.name,
            surname: user.surname,
          },
        };
        storage.setItem('user', JSON.stringify(USER_INFO));
        storage.setItem('isAuthenticated', 'true');
        this.authChanged.emit();
        this.router.navigate(['']);
        if (USER_INFO.info.role === 1) {
          this.router.navigate(['/admin/films']);
        }
      },
      error: (e: HttpErrorResponse) => {
        this.errors.emit(e);
      },
    });
  }

  logout() {
    localStorage.removeItem('user');
    localStorage.removeItem('isAuthenticated');

    this.authChanged.emit();
    this.router.navigate(['']);
  }

  isAuthenticated(): boolean {
    return localStorage.getItem('isAuthenticated') == 'true';
  }

  getUserInfo(): UserInfo {
    return JSON.parse(localStorage.getItem('user')!) as UserInfo;
  }
}
