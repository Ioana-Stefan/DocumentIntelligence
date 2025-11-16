import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError, Observable, of } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';
import { Router } from '@angular/router';

export const TokenInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<any> => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const accessToken = localStorage.getItem('accessToken');
  let clonedReq = req;

  if (accessToken) {
    clonedReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${accessToken}`)
    });
  }

  return next(clonedReq).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401) {
        const refreshToken = localStorage.getItem('refreshToken');

        if (!refreshToken) {
          router.navigate(['/login']);
          return throwError(() => err);
        }

        // Call refreshToken from AuthService
        return authService.refreshToken(refreshToken).pipe(
          switchMap(response => {
            // Store new tokens
            localStorage.setItem('accessToken', response.accessToken);
            if (response.refreshToken) {
              localStorage.setItem('refreshToken', response.refreshToken);
            }

            // Retry the failed request with new access token
            const retryReq = req.clone({
              headers: req.headers.set('Authorization', `Bearer ${response.accessToken}`)
            });

            return next(retryReq);
          }),
          catchError(() => {
            // Refresh failed → redirect to login
            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');
            router.navigate(['/login']);
            return throwError(() => err);
          })
        );
      }

      // Other errors → just pass through
      return throwError(() => err);
    })
  );
};
