import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { AuthService } from '../../services/auth';
import { LoginDto } from '../../dtos/login-dto';

@Component({
  selector: 'app-login',
  imports: [FormsModule, MatButtonModule, MatCardModule, MatFormFieldModule, MatInputModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  private authService = inject(AuthService);
  private router = inject(Router);

  credentials: LoginDto = {
    name: '',
    password: ''
  };

  isLoading = signal(false);
  errorMessage = signal('');

  login(): void {

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.authService.login(this.credentials)
      .subscribe({
        next: () => {
          this.credentials.password = '';
          this.isLoading.set(false);
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          console.error('Login error:', err);
          console.error('HTTP status:', err.status);
          console.error('Response:', err.error);

          this.errorMessage.set(
            `Login failed: ${err.status} - ${err.statusText}`
          );

          this.isLoading.set(false);
        }
      });
  }
}