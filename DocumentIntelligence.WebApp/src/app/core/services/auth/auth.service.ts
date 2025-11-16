import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserResponse } from '../../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = '/api/v1/auth';

  constructor(private http: HttpClient) {}

  login(dto: { email: string; password: string }): Observable<UserResponse> {
    return this.http.post<UserResponse>(`${this.apiUrl}/login`, dto);
  }

  refreshToken(refreshToken: string): Observable<UserResponse> {
    return this.http.post<UserResponse>(`${this.apiUrl}/refresh`, { token: refreshToken });
  }

  logout(refreshToken: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/revoke`, { token: refreshToken });
  }
}
