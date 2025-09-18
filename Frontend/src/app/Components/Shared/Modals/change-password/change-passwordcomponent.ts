import { Component, Input, Output, EventEmitter, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
<<<<<<< HEAD:Frontend/src/app/Components/Shared/Modals/change-password/change-passwordcomponent.ts
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import Swal from 'sweetalert2';
import { AlertService } from '../../../../Core/Service/alert.service';
import { AuthService } from '../../../../Core/Service/Auth/auth.service';
import { UserService } from '../../../../Core/Service/SecurityModule/user.service';
import { performLogout } from '../../../../Core/Utils/auth.util';
=======

import Swal from 'sweetalert2';
import { AuthService } from '../../../../../Core/Service/Auth/auth.service';
import { Router, RouterLink } from '@angular/router';
import { confirmLogout, successMessage } from '../../../../../Core/Utils/alerts.util';
import { UserService } from '../../../../../Core/Service/SecurityModule/user.service';
>>>>>>> parent of 845d2803 (solucion de errores):Frontend/src/app/Components/System/Admin/Modal/admin-change-password/admin-change-passwordcomponent.ts


@Component({
	selector: 'app-change-password-modal',
	standalone: true,
	imports: [
		CommonModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatIconModule,
		RouterLink
	],
	templateUrl: './change-password.component.html',
	styleUrls: ['../../../Shared/Styles/modal-shared.css', './change-password.component.css']
})
export class ChangePasswordModalComponent implements OnInit {
	private readonly formBuilder = inject(FormBuilder);
	private readonly userService = inject(UserService);
	private readonly authService = inject(AuthService);
	private readonly router = inject(Router);
	private readonly alertService = inject(AlertService);

	@Input({ required: true }) isOpen = false;
	@Output() onClose = new EventEmitter<void>();
	@Output() onSuccess = new EventEmitter<void>();

	passwordForm!: FormGroup;
	isChangingPassword = signal(false);
	showCurrentPassword = signal(false);
	showNewPassword = signal(false);
	showConfirmPassword = signal(false);

	newPassword = signal('');
	hasUpperCase = computed(() => /[A-Z]/.test(this.newPassword()));
	hasLowerCase = computed(() => /[a-z]/.test(this.newPassword()));
	hasNumber = computed(() => /[0-9]/.test(this.newPassword()));
	hasSpecialChar = computed(() => /[!@#$%^&*(),.?":{}|<>]/.test(this.newPassword()));
	hasMinLength = computed(() => this.newPassword().length >= 8);

	ngOnInit(): void {
		this.initForm();
	}

	private initForm(): void {
		this.passwordForm = this.formBuilder.group({
			currentPassword: ['', [Validators.required, Validators.minLength(6)]],
			newPassword: ['', [Validators.required, this.passwordStrengthValidator]],
			confirmPassword: ['', [Validators.required]]
		}, {
			validators: this.passwordMatchValidator
		});

		this.passwordForm.get('newPassword')?.valueChanges.subscribe(value => {
			this.newPassword.set(value || '');
		});
	}

	private passwordStrengthValidator(control: AbstractControl): ValidationErrors | null {
		const password = control.value;
		if (!password) return null;

		const hasUpperCase = /[A-Z]/.test(password);
		const hasLowerCase = /[a-z]/.test(password);
		const hasNumber = /[0-9]/.test(password);
		const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(password);
		const hasMinLength = password.length >= 8;

		const valid = hasUpperCase && hasLowerCase && hasNumber && hasSpecialChar && hasMinLength;

		return valid ? null : { passwordStrength: true };
	}

	private passwordMatchValidator(form: AbstractControl): ValidationErrors | null {
		const newPassword = form.get('newPassword')?.value;
		const confirmPassword = form.get('confirmPassword')?.value;

		return newPassword === confirmPassword ? null : { passwordMismatch: true };
	}

<<<<<<< HEAD:Frontend/src/app/Components/Shared/Modals/change-password/change-passwordcomponent.ts
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

=======
>>>>>>> parent of 845d2803 (solucion de errores):Frontend/src/app/Components/System/Admin/Modal/admin-change-password/admin-change-passwordcomponent.ts
	closeModal(): void {
		if (this.isChangingPassword()) return;

		this.passwordForm.reset();
		this.newPassword.set('');
		this.onClose.emit();
	}

	changePassword(): void {
		if (this.passwordForm.invalid) {
			this.markFormGroupTouched(this.passwordForm);
			return;
		}

		const { currentPassword, newPassword } = this.passwordForm.value;

		this.alertService.confirmWithLoading(
			'Se actualizará la contraseña actual',
			() => {
				this.isChangingPassword.set(true);
				return lastValueFrom(this.userService.changePassword(currentPassword, newPassword))
					.then(() => {
						this.passwordForm.reset();
						performLogout(this.router);
						this.onSuccess.emit();
					})
					.finally(() => {
						this.isChangingPassword.set(false);
					});
			},
			{
				successTitle: '¡Contraseña cambiada!',
				successText: 'Tu contraseña ha sido actualizada correctamente',
				errorTitle: 'Error',
				errorText: 'Error al cambiar contraseña'
			}
		);
	}

	private markFormGroupTouched(formGroup: FormGroup): void {
		Object.keys(formGroup.controls).forEach(key => {
			const control = formGroup.get(key);
			control?.markAsTouched();
		});
	}

	togglePasswordVisibility(field: 'current' | 'new' | 'confirm'): void {
		switch (field) {
			case 'current':
				this.showCurrentPassword.update(show => !show);
				break;
			case 'new':
				this.showNewPassword.update(show => !show);
				break;
			case 'confirm':
				this.showConfirmPassword.update(show => !show);
				break;
		}
	}

	get currentPasswordErrors(): string[] {
		const control = this.passwordForm.get('currentPassword');
		const errors: string[] = [];

		if (control?.errors && control.touched) {
			if (control.errors['required']) errors.push('La contraseña actual es requerida');
		}

		return errors;
	}

	get newPasswordErrors(): string[] {
		const control = this.passwordForm.get('newPassword');
		const errors: string[] = [];

		if (control?.errors && control.touched) {
			if (control.errors['required']) errors.push('La nueva contraseña es requerida');
			if (control.errors['passwordStrength']) errors.push('La contraseña no cumple los requisitos de seguridad');
		}

		return errors;
	}

	get confirmPasswordErrors(): string[] {
		const errors: string[] = [];
		const form = this.passwordForm;

		if (form?.errors?.['passwordMismatch'] && form.get('confirmPassword')?.touched) {
			errors.push('Las contraseñas no coinciden');
		}

		return errors;
	}
}
