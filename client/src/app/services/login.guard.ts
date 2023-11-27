import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const loginGuard: CanActivateFn = (route, state) => {
  const isAuth = !!localStorage.getItem('isAuthenticated')!;

  if (isAuth) {
    const router = inject(Router);
    router.navigate(['/error']);
    return false;
  }
  return true;
};
