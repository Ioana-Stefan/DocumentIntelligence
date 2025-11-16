import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError, Observable, of } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';


export const tokenInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const authService = inject(AuthService);
  const token = authService.getAccessToken();

  let authReq = req;
  if (token) {
    authReq = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }

  return next(authReq).pipe(
    catchError(err => {
      if (err.status === 401) {
        const refreshToken = authService.getRefreshToken();
        if (refreshToken) {
          return authService.refreshToken(refreshToken).pipe(
            switchMap(user => {
              const clonedReq = req.clone({ setHeaders: { Authorization: `Bearer ${user.accessToken}` } });
              return next(clonedReq);
            }),
            catchError(() => {
              authService.logout();
              return throwError(() => err);
            })
          );
        } else {
          authService.logout();
        }
      }
      return throwError(() => err);
    })
  );
};
