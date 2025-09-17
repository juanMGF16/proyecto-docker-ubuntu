import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { InitialHeaderComponent } from "../../../Components/System/Landing/initial-header/initial-navbar.component";
import { AuthService } from '../../../Core/Service/Auth/auth.service';
import { RoleRedirectService } from '../../../Core/Service/Auth/role-redirect.service';
import { AlertService } from '../../../Core/Service/alert.service';
import { lastValueFrom } from 'rxjs';

@Component({
	selector: 'app-login',
	standalone: true,
	imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, RouterLink, MatIconModule, InitialHeaderComponent],
	templateUrl: './login.component.html',
	styleUrl: './login.component.css'
})
export class LoginComponent {

	private formBuilder = inject(FormBuilder);
	private authService = inject(AuthService);
	private roleRedirect = inject(RoleRedirectService);
	private alertService = inject(AlertService);

	hidePassword = true;
	usernameFocused = false;
	passwordFocused = false;
	isLoggingIn = signal(false); // Deshabilitar Boton Submit

	loginForm = this.formBuilder.group({
		username: ['', Validators.required],
		password: ['', [
			Validators.required
		]]
	});

	async onSubmit(): Promise<void> {
		if (this.loginForm.invalid || this.isLoggingIn()) {
			this.loginForm.markAllAsTouched();
			return;
		}

		const { username, password } = this.loginForm.value;

		this.isLoggingIn.set(true); // Deshabilitar Boton Submit
		this.alertService.showLoading('Procesando...', 'Iniciando sesión');

		try {
			const loginResponse = await lastValueFrom(
				this.authService.login({ username: username!, password: password! })
			);

			this.alertService.closeAlert();
			this.authService.saveToken(loginResponse.token);
			const role = this.authService.getRole();
			this.roleRedirect.redirectUser(role);

		} catch (error: any) {
			this.alertService.closeAlert();

			Swal.fire({
				icon: 'error',
				title: 'Oopss...',
				text: 'Credenciales Incorrectas',
				confirmButtonText: 'Aceptar'
			});
		} finally {
			this.isLoggingIn.set(false); //  Rehabilitar botón
		}
	}

	onForgotPassword(): void {
		Swal.fire({
			title: 'Recuperar contraseña',
			text: 'Ingresa tu correo electrónico',
			input: 'email',
			inputPlaceholder: 'correo@ejemplo.com',
			showCancelButton: true,
			confirmButtonText: 'Enviar',
			cancelButtonText: 'Cancelar',
			inputValidator: (value) => {
				if (!value) {
					return 'Por favor ingresa tu correo';
				}
				return null;
			}
		}).then((result) => {
			if (result.isConfirmed && result.value) {
				const email = result.value;

				this.alertService.withLoading(
					() => lastValueFrom(this.authService.forgotPassword(email)),
					{
						successTitle: 'Solicitud enviada',
						successText: 'Si el email está registrado, recibirás instrucciones en tu bandeja de entrada 📩',
						errorTitle: 'Error',
						errorText: 'Ocurrió un error al procesar la solicitud'
					}
				);
			}
		});
	}
}
