import { Component, Input, Output, EventEmitter, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import Swal from 'sweetalert2';
import { AuthService } from '../../../../../Core/Service/Auth/auth.service';
import { Router, RouterLink } from '@angular/router';
import { confirmLogout, successMessage } from '../../../../../Core/Utils/alerts.util';
import { UserService } from '../../../../../Core/Service/SecurityModule/user.service';


@Component({
	selector: 'app-admin-change-password-modal',
	standalone: true,
	imports: [
		CommonModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatIconModule,
		RouterLink
	],
	templateUrl: './admin-change-password.component.html',
	styleUrls: ['../../../../Shared/Styles/modal-shared.css', './admin-change-password.component.css']
})
export class AdminChangePasswordModalComponent implements OnInit {
	private readonly formBuilder = inject(FormBuilder);
	private readonly userService = inject(UserService);
	private readonly authService = inject(AuthService);
	private readonly router = inject(Router);

	@Input({ required: true }) isOpen = false;
	@Output() onClose = new EventEmitter<void>();
	@Output() onSuccess = new EventEmitter<void>();

	passwordForm!: FormGroup;
	isChangingPassword = signal(false);
	showCurrentPassword = signal(false);
	showNewPassword = signal(false);
	showConfirmPassword = signal(false);

	// Computed signals para validaciones de contraseña
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

		// Actualizar signal cuando cambie la nueva contraseña
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

		Swal.fire({
			title: '¿Confirmar cambios?',
			text: 'Se actualizará la contraseña actual',
			icon: 'question',
			showCancelButton: true,
			confirmButtonText: 'Sí, actualizar',
			cancelButtonText: 'Cancelar',
			showLoaderOnConfirm: true,
			preConfirm: () => {
				this.isChangingPassword.set(true);
				return this.userService.changePassword(currentPassword, newPassword).toPromise()
					.then(() => {
						this.isChangingPassword.set(false);
						this.passwordForm.reset();
						this.newPassword.set('');
						this.authService.logout();
						successMessage("Sesión cerrada");
						this.onSuccess.emit();
					})
					.catch(error => {
						this.isChangingPassword.set(false);
						Swal.showValidationMessage(
							error.error?.message || 'Error al cambiar contraseña'
						);
					});
			},
			allowOutsideClick: () => !Swal.isLoading()
		});
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

	// Getters para errores
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
