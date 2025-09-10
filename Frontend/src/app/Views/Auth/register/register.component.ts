import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { ColombianPhoneDirective } from '../../../Components/Shared/Directives/colombian-phone.directive';
import { OnlyNumbersDirective } from '../../../Components/Shared/Directives/only-numbers.directive';
import { InitialHeaderComponent } from "../../../Components/System/Landing/initial-header/initial-navbar.component";
import { AuthService } from '../../../Core/Service/Auth/auth.service';
import { colombianPhoneValidator, emailValidator, strongPassword } from '../../../Core/Utils/input-validators.util';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    RouterLink,
    OnlyNumbersDirective,
    ColombianPhoneDirective,
    InitialHeaderComponent
],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);

  hidePassword = true;
  isSubmit = signal(false);
  wasSubmitted = false;

  registerForm = this.formBuilder.nonNullable.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    password: ['', [Validators.required, Validators.minLength(8), strongPassword()]],
    name: ['', [Validators.required, Validators.minLength(3)]],
    lastName: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, emailValidator()]],
    documentType: ['', Validators.required],
    documentNumber: ['', [
      Validators.required,
      Validators.pattern(/^[0-9]{6,10}$/),
    ]],
    phone: ['', [
      Validators.required,
      colombianPhoneValidator()
    ]],
  });

  documentTypes = [
    { value: 'RC', label: 'Registro Civil' },
    { value: 'TI', label: 'Tarjeta de Identidad' },
    { value: 'CC', label: 'Cédula de Ciudadanía' },
    { value: 'CE', label: 'Cédula de Extranjería' },
    { value: 'NIT', label: 'NIT' },
    { value: 'PP', label: 'Pasaporte' }
  ];

  onSubmit(): void {
    this.wasSubmitted = true;

    if (this.registerForm.invalid || this.isSubmit()) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isSubmit.set(true);

    const {
      username,
      password,
      name,
      lastName,
      email,
      documentType,
      documentNumber,
      phone,
    } = this.registerForm.getRawValue();

    this.authService.register({
      username,
      password,
      name,
      lastName,
      email: email.trim().toLowerCase(),
      documentType,
      documentNumber,
      phone,
    }).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Registro Exitoso',
          text: 'Tu cuenta ha sido creada correctamente'
        });

        this.router.navigate(['/Login']);
      },
      error: (err) => {
        console.log('Error al registrar:', err);
        const mensajeCompleto = err?.error?.error || 'Ocurrió un error inesperado.';
        const mensaje = mensajeCompleto.split(':')[1]?.trim() || mensajeCompleto;
        Swal.fire({
          icon: 'error',
          title: 'Error en el registro',
          text: mensaje,
          confirmButtonText: 'Aceptar'
        });
        this.isSubmit.set(false);
      },
      complete: () => {
        this.isSubmit.set(false);
      },
    });
  }
}
