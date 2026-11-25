import { inject } from '@angular/core';

import {
  CanActivateChildFn,
  Router
} from '@angular/router';

import { map } from 'rxjs';
import { AuthService } from '../services/auth';

export const authGuard: CanActivateChildFn = () => {

  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.isLoggedIn().pipe(
    map(isLoggedIn => {

      if (isLoggedIn) {
        return true;
      }

      return router.createUrlTree(['/login']);
    })
  );
};