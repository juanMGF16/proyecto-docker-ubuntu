import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../../../../Core/Service/Auth/auth.service';
import Swal from 'sweetalert2';
import { CompanyService } from '../../../../../Core/Service/System/company.service';
import { emailValidator } from '../../../../../Core/Utils/input-validators.util';
import { CompanyOptionsMod } from '../../../../../Core/Models/System/CompanyMod.model';
import { OnlyNumbersDirective } from '../../../../../Core/Directives/only-numbers.directive';


@Component({
	selector: 'app-register-company',
	standalone: true,
	imports: [
		CommonModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatIconModule,
		RouterLink,
		OnlyNumbersDirective
	],
	templateUrl: './register-company.component.html',
	styleUrls: ['./register-company.component.css']
})
export class RegisterCompanyComponent implements OnInit {
	private authService = inject(AuthService);
	private companyService = inject(CompanyService);
	private formBuilder = inject(FormBuilder);
	private router = inject(Router);

	isSubmit = signal(false);
	wasSubmitted = false;
	idUser: number = 0;
	companyForm!: FormGroup;

	industries = [{ id: 1, name: 'Tecnología' }, { id: 2, name: 'Manufactura' }, { id: 3, name: 'Retail' }, { id: 4, name: 'Salud' },
	{ id: 5, name: 'Educación' }, { id: 6, name: 'Finanzas' }, { id: 7, name: 'Construcción' }, { id: 8, name: 'Transporte' },
	{ id: 9, name: 'Agricultura' }, { id: 10, name: 'Otro' }
	];

	private nitValidator(control: AbstractControl): ValidationErrors | null {
		const value = control.value;

		if (!value) return null;

		// Patrón: exactamente 9 dígitos numéricos
		const nitPattern = /^\d{9}$/;

		if (!nitPattern.test(value)) {
			return { invalidNIT: true };
		}

		return null;
	}


	ngOnInit(): void {
		const userIdString = this.authService.getIdUser();
		this.idUser = parseInt(userIdString, 10);

		this.companyForm = this.formBuilder.nonNullable.group({
			name: ['', [Validators.required, Validators.minLength(3)]],
			businessName: ['', [Validators.required, Validators.minLength(3)]],
			nit: ['', [Validators.required, this.nitValidator.bind(this)]],
			industryId: ['', Validators.required],
			email: ['', [Validators.required, emailValidator()]],
			website: ['', Validators.pattern(/^(https?:\/\/)?([\da-z.-]+)\.([a-z.]{2,6})([/\w .-]*)*\/?$/)],
			termsAccepted: [false, Validators.requiredTrue],
			userId: [this.idUser]
		});
	}

	onSubmit(): void {
		this.wasSubmitted = true;

		if (this.companyForm.invalid || this.isSubmit()) {
			this.companyForm.markAllAsTouched();
			return;
		}

		this.isSubmit.set(true);

		const formData = this.companyForm.getRawValue();
		const companyData: CompanyOptionsMod = {
			name: formData.name,
			businessName: formData.businessName,
			nit: formData.nit,
			industryId: formData.industryId,
			email: formData.email,
			website: formData.website || undefined,
			userId: formData.userId
		};

		this.companyService.registerCompany(companyData).subscribe({
			next: (response) => {
				this.isSubmit.set(false);

				Swal.fire({
					icon: 'success',
					title: '¡Empresa registrada exitosamente!',
					confirmButtonText: 'Continuar',
					confirmButtonColor: '#28a745'
				}).then((result) => {
					if (result.isConfirmed) {
						this.router.navigate(['/admin/dashboard/']);
					}
				});
			},
			error: (error) => {
				this.isSubmit.set(false);

				let errorMessage = 'Ha ocurrido un error inesperado. Por favor, inténtalo de nuevo.';

				if (error.error?.message) {
					errorMessage = error.error.message;
				} else if (error.status === 400) {
					errorMessage = 'Los datos ingresados no son válidos. Por favor, verifica la información.';
				} else if (error.status === 500) {
					errorMessage = 'Error interno del servidor. Por favor, contacta al administrador.';
				}

				Swal.fire({
					icon: 'error',
					title: 'Error al registrar empresa',
					text: errorMessage,
					confirmButtonText: 'Entendido',
					confirmButtonColor: '#dc3545'
				});
			}
		});
	}

	onCancel(): void {
		if (this.companyForm.dirty) {
			Swal.fire({
				title: '¿Estás seguro?',
				text: 'Se perderán todos los datos ingresados.',
				icon: 'warning',
				showCancelButton: true,
				confirmButtonColor: '#dc3545',
				cancelButtonColor: '#6c757d',
				confirmButtonText: 'Sí, cancelar',
				cancelButtonText: 'No, continuar'
			}).then((result) => {
				if (result.isConfirmed) {
					this.router.navigate(['/admin/welcome']);
				}
			});
		} else {
			this.router.navigate(['/admin/welcome']);
		}
	}

	// Método auxiliar para obtener mensajes de error de los campos
	getFieldError(fieldName: string): string {
		const field = this.companyForm.get(fieldName);
		if (field?.errors && (field.dirty || field.touched || this.wasSubmitted)) {
			if (field.errors['required']) return 'Este campo es requerido';
			if (field.errors['minlength']) return `Mínimo ${field.errors['minlength'].requiredLength} caracteres`;
			if (field.errors['email']) return 'Email debe tener un formato válido';
			if (field.errors['invalidNIT']) return 'El NIT debe tener exactamente 9 dígitos';
			if (field.errors['pattern']) {
				if (fieldName === 'website') return 'Formato de URL inválido';
			}
		}
		return '';
	}

	// Método para verificar si un campo tiene errores
	hasFieldError(fieldName: string): boolean {
		const field = this.companyForm.get(fieldName);
		return !!(field?.errors && (field.dirty || field.touched || this.wasSubmitted));
	}
}
