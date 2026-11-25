import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import {
  Observable,
  map,
  catchError,
  of,
  switchMap
} from 'rxjs';

import { LoginDto } from '../dtos/login-dto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient);
  private apiUrl = '/api/auth';

  getCsrfToken(): Observable<void> {
    return this.http.get<void>(
      `${this.apiUrl}/csrf`
    );
  }

  login(dto: LoginDto): Observable<void> {
    return this.getCsrfToken().pipe(
      switchMap(() =>
        this.http.post(
          `${this.apiUrl}/login`,
          dto
        )
      ),
      switchMap(() => this.getCsrfToken())
    );
  }

  isLoggedIn(): Observable<boolean> {
    return this.http.get(
      `${this.apiUrl}/me`
    ).pipe(
      map(() => true),
      catchError(error => {
        console.error('Authentication check failed:', error);
        return of(false);
      })
    );
  }

  logout(): Observable<void> {
    return this.getCsrfToken().pipe(
      switchMap(() =>
        this.http.post<void>(
          `${this.apiUrl}/logout`,
          {}
        )
      ),
      switchMap(() => this.getCsrfToken())
    );
  }
}