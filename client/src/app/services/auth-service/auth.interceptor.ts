import { Injectable } from '@angular/core';
import { AuthService, UserInfo } from './auth.service';
import { HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler,
  ): Observable<HttpEvent<unknown>> {
    const isLoggedIn = this.authService.isAuthenticated();
    const userInfo: UserInfo = this.authService.getUserInfo();

    if (isLoggedIn) {
      request = request.clone({
        setHeaders: { Authorization: `Basic ${userInfo.authData}` },
      });
    }

    return next.handle(request);
  }
}
