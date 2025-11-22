import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserRegistrationResponse } from '../../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {
  private readonly apiUrl = '/api/v1/users/register';

  constructor(private http: HttpClient) {}

  register(email: string, name: string, password: string): Observable<UserRegistrationResponse> {
    return this.http.post<UserRegistrationResponse>(this.apiUrl, { email, name, password });
  }
}
