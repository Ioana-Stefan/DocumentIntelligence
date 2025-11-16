import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { UserResponse } from '../../models/user.model';
import { tap, map, catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = '/api/v1/auth';

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<UserResponse> {
    return this.http.post<UserResponse>(`${this.apiUrl}/login`, { email, password }).pipe(
      tap(user => this.setTokens(user.accessToken, user.refreshToken))
    );
  }

  refreshToken(refreshToken: string): Observable<UserResponse> {
    return this.http.post<UserResponse>(`${this.apiUrl}/refresh`, { token: refreshToken }).pipe(
      tap(user => this.setTokens(user.accessToken, user.refreshToken))
    );
  }

  validateToken(): Observable<boolean> {
    const token = localStorage.getItem('accessToken');
    if (!token) return of(false);

    return this.http.post<{ valid: boolean }>(`${this.apiUrl}/validate`, { token }).pipe(
      map(res => res.valid),
      catchError(() => of(false))
    );
  }

  logout(): void {
    const refreshToken = localStorage.getItem('refreshToken');
    if (refreshToken) {
      this.http.post(`${this.apiUrl}/revoke`, { token: refreshToken }).subscribe();
    }
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
  }

  private setTokens(accessToken: string, refreshToken?: string) {
    localStorage.setItem('accessToken', accessToken);
    if (refreshToken) localStorage.setItem('refreshToken', refreshToken);
  }

  getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }
}
