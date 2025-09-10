import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import Swal from 'sweetalert2';
import { UserService } from '../../../../../Core/Service/SecurityModule/user.service';
import { UserPartialUpdateMod } from '../../../../../Core/Models/SecurityModule/UserMod.model';
import { AuthService } from '../../../../../Core/Service/Auth/auth.service';
import { colombianPhoneValidator, emailValidator } from '../../../../../Core/Utils/input-validators.util';

@Component({
	selector: 'app-admin-edit-profile-modal',
	standalone: true,
	imports: [
		CommonModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatIconModule
	],
	templateUrl: './admin-edit-profile.component.html',
	styleUrls: ['../../../../Shared/Styles/modal-shared.css', './admin-edit-profile.component.css']
})
export class AdminEditProfileModalComponent implements OnInit, OnChanges {
	private readonly formBuilder = inject(FormBuilder);
	private readonly userService = inject(UserService);
	private readonly authService = inject(AuthService);

	@Input({ required: true }) user!: UserPartialUpdateMod;
	@Input({ required: true }) isOpen = false;
	@Output() onClose = new EventEmitter<void>();
	@Output() onSave = new EventEmitter<any>();

	profileForm!: FormGroup;
	isSaving = signal(false);

	ngOnInit(): void {
		this.initForm();
	}

	ngOnChanges(changes: SimpleChanges): void {
		if (changes['user'] && this.profileForm) {
			this.updateFormValues();
		}
	}

	private initForm(): void {
		this.profileForm = this.formBuilder.group({
			email: [
				this.user?.email || '',
				[Validators.required, emailValidator()]
			],
			phone: [
				this.user?.phone || '',
				[Validators.required, colombianPhoneValidator()]
			],
			username: [
				this.authService.getUsername() || this.user?.username || '',
				[Validators.required, Validators.minLength(3), Validators.maxLength(50)]
			],
		});
	}


	private updateFormValues(): void {
		this.profileForm.patchValue({
			email: this.user?.email || '',
			phone: this.user?.phone || '',
			username: this.user?.username || ''
		});
	}

	closeModal(): void {
		if (this.isSaving()) return;

		this.profileForm.reset();
		this.onClose.emit();
	}

	saveProfile(): void {
		if (this.profileForm.invalid) {
			this.markFormGroupTouched(this.profileForm);
			return;
		}

		const formData = {
			...this.profileForm.getRawValue(),
			id: this.authService.getIdUser()
		};

		Swal.fire({
			title: '¿Confirmar cambios?',
			text: 'Se actualizará la información de tu perfil',
			icon: 'question',
			showCancelButton: true,
			confirmButtonText: 'Sí, actualizar',
			cancelButtonText: 'Cancelar'
		}).then((result) => {
			if (result.isConfirmed) {
				this.isSaving.set(true);

				this.userService.partialUpdate(formData).subscribe({
					next: (updatedUser) => {
						this.isSaving.set(false);
						Swal.fire({
							title: '¡Éxito!',
							text: 'Perfil actualizado correctamente',
							icon: 'success',
							timer: 2000,
							showConfirmButton: false
						});
						this.onSave.emit(updatedUser);
					},
					error: (error) => {
						this.isSaving.set(false);
						Swal.fire({
							title: 'Error',
							text: error.error?.message || 'No se pudo actualizar el perfil',
							icon: 'error',
							confirmButtonText: 'Entendido'
						});
					}
				});
			}
		});
	}



	private markFormGroupTouched(formGroup: FormGroup): void {
		Object.keys(formGroup.controls).forEach(key => {
			const control = formGroup.get(key);
			control?.markAsTouched();
		});
	}

	get emailErrors(): string[] {
		const control = this.profileForm.get('email');
		const errors: string[] = [];

		if (control?.errors && control.touched) {
			if (control.errors['required']) errors.push('El email es requerido');
			if (control.errors['emailFormat']) errors.push('Formato de email inválido');
		}

		return errors;
	}

	get phoneErrors(): string[] {
		const control = this.profileForm.get('phone');
		const errors: string[] = [];

		if (control?.errors && control.touched) {
			if (control.errors['required']) errors.push('El teléfono es requerido');
			if (control.errors['colombianPhone']) errors.push(control.errors['colombianPhone']);
		}

		return errors;
	}

	get usernameErrors(): string[] {
		const control = this.profileForm.get('username');
		const errors: string[] = [];

		if (control?.errors && control.touched) {
			if (control.errors['required']) errors.push('El username es requerido');
			if (control.errors['minlength']) errors.push('Mínimo 3 caracteres');
			if (control.errors['maxlength']) errors.push('Máximo 50 caracteres');
		}

		return errors;
	}
}
