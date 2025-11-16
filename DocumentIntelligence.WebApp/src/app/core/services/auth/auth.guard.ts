import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = async (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);

  const isValid = await firstValueFrom(authService.validateToken());

  if (!isValid) {
    router.navigate(['/login']);
    return false;
  }

  return true;
};
