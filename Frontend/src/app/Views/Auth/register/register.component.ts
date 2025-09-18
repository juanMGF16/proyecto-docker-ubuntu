import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterLink } from '@angular/router';
<<<<<<< HEAD
import { lastValueFrom } from 'rxjs';
=======
import Swal from 'sweetalert2';
import { ColombianPhoneDirective } from '../../../Components/Shared/Directives/colombian-phone.directive';
import { OnlyNumbersDirective } from '../../../Components/Shared/Directives/only-numbers.directive';
>>>>>>> parent of 845d2803 (solucion de errores)
import { InitialHeaderComponent } from "../../../Components/System/Landing/initial-header/initial-navbar.component";
import { NumericInputDirective } from '../../../Core/Directives/numeric-input.directive';
import { AlertService } from '../../../Core/Service/alert.service';
import { AuthService } from '../../../Core/Service/Auth/auth.service';
<<<<<<< HEAD
import { colombianPhoneValidator, documentNumberValidator, emailValidator, strongPassword } from '../../../Core/Utils/input-validators.util';
=======
import { colombianPhoneValidator, emailValidator, strongPassword } from '../../../Core/Utils/input-validators.util';
>>>>>>> parent of 845d2803 (solucion de errores)

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
		InitialHeaderComponent,
		NumericInputDirective
	],
	templateUrl: './register.component.html',
	styleUrl: './register.component.css'
})
export class RegisterComponent {
	private authService = inject(AuthService);
	private formBuilder = inject(FormBuilder);
	private router = inject(Router);
	private alertService = inject(AlertService);

	hidePassword = true;
	isSubmitting = signal(false);
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
			documentNumberValidator(6, 10)
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
		{ value: 'PP', label: 'Pasaporte' }
	];

	async onSubmit(): Promise<void> {
		this.wasSubmitted = true;

		if (this.registerForm.invalid || this.isSubmitting()) {
			this.registerForm.markAllAsTouched();
			return;
		}

		this.isSubmitting.set(true);

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

<<<<<<< HEAD
		try {
			const result = await this.alertService.withLoading(
				() => lastValueFrom(this.authService.register({
					username: username!,
					password: password!,
					name: name!,
					lastName: lastName!,
					email: email!.trim().toLowerCase(),
					documentType: documentType!,
					documentNumber: documentNumber!,
					phone: phone!,
				})),
				{
					successTitle: 'Registro Exitoso',
					successText: 'Registro exitoso. Revisa tu correo electrónico 📩',
					errorTitle: 'Error en el registro',
					errorText: 'Ocurrió un error inesperado'
				}
			);
=======
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
>>>>>>> parent of 845d2803 (solucion de errores)

			if (result.isConfirmed) {
				this.router.navigate(['/Login']);
			}

		} catch (error: any) {
			console.error('Error completo al registrar:', error);

			if (error?.error?.error) {
				const mensajeEspecifico = error.error.error;
				console.log('Mensaje de error de la API:', mensajeEspecifico);
			}
		} finally {
			this.isSubmitting.set(false);
		}
	}
}
