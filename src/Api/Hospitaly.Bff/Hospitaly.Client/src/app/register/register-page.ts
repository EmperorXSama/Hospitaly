import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RegistrationService } from '../services/registration';
import { AuthService } from '../services/auth';

@Component({
  selector: 'app-register-page',
  imports: [FormsModule],
  templateUrl: './register-page.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterPage {
  readonly router = inject(Router);
  private readonly registrationService = inject(RegistrationService);
  private readonly authService = inject(AuthService);

  email = '';
  password = '';
  firstName = '';
  lastName = '';
  sex = '';
  dateOfBirth = '';
  bloodType = '';
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  submit(): void {
    this.errorMessage.set(null);
    this.isLoading.set(true);

    this.registrationService
      .register({
        email: this.email,
        password: this.password,
        firstName: this.firstName,
        lastName: this.lastName,
        sex: this.sex,
        dateOfBirth: this.dateOfBirth,
        bloodType: this.bloodType || null,
      })
      .subscribe({
        next: (response) => {
          this.isLoading.set(false);
          if (response.isSuccess && response.data) {
            this.authService.login();
          } else {
            const err = response.error as { message?: string } | undefined;
            this.errorMessage.set(err?.message ?? 'Registration failed. Please try again.');
          }
        },
        error: () => {
          this.isLoading.set(false);
          this.errorMessage.set('Something went wrong. Please try again.');
        },
      });
  }
}
