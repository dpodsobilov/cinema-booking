import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const loginGuard: CanActivateFn = (route, state) => {
  let isAuth = !!localStorage.getItem('isAuthenticated')!;

  if (isAuth) {
    let router = inject(Router);
    router.navigate(['/error']);
    return false;
  }
  return true;
};
