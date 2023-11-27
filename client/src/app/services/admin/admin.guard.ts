import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const adminGuard: CanActivateFn = (route, state) => {
  const isAuth = !!localStorage.getItem('isAuthenticated')!;
  let isAdmin = false;
  if (isAuth) {
    isAdmin = !!JSON.parse(localStorage.getItem('user')!).info.role;
  }
  if (!isAuth || !isAdmin) {
    const router = inject(Router);
    router.navigate(['/error']);
    return false;
  }
  return true;
};
