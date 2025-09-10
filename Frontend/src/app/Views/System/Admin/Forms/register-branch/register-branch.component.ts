import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators, FormGroup } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { ColombianPhoneDirective } from '../../../../../Core/Directives/colombian-phone.directive';
import { colombianPhoneValidator, emailValidator } from '../../../../../Core/Utils/input-validators.util';
import { mixedPhoneValidator } from '../../../../../Core/Directives/mixed-phone.validator';
import { OnlyNumbersDirective } from "../../../../../Core/Directives/only-numbers.directive";

@Component({
	selector: 'app-register-branch',
	standalone: true,
	imports: [
		CommonModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatIconModule,
		MatStepperModule,
		ColombianPhoneDirective,
		OnlyNumbersDirective
	],
	templateUrl: './register-branch.component.html',
	styleUrl: './register-branch.component.css'
})
export class RegisterBranchComponent {
	private formBuilder = inject(FormBuilder);
	private router = inject(Router);

	isSubmit = signal(false);
	wasSubmitted = false;
	currentStep = signal(0);

	branchForm = this.formBuilder.nonNullable.group({
		// Sección Sucursal
		branchName: ['', [Validators.required, Validators.minLength(3)]],
		branchAddress: ['', [Validators.required, Validators.minLength(5)]],
		branchPhone: ['', [Validators.required, mixedPhoneValidator()]], //

		// Sección Subadministrador
		adminName: ['', [Validators.required, Validators.minLength(3)]],
		adminLastName: ['', [Validators.required, Validators.minLength(3)]],
		adminDocumentType: ['', Validators.required],
		adminDocumentNumber: ['', [Validators.required, Validators.pattern(/^[0-9]{6,10}$/),]],
		adminPhone: ['', [Validators.required, colombianPhoneValidator()]],
		adminEmail: ['', [Validators.required, emailValidator()]]
	});

	documentTypes = [
		{ value: 'RC', label: 'Registro Civil' },
		{ value: 'TI', label: 'Tarjeta de Identidad' },
		{ value: 'CC', label: 'Cédula de Ciudadanía' },
		{ value: 'CE', label: 'Cédula de Extranjería' },
		{ value: 'NIT', label: 'NIT' },
		{ value: 'PP', label: 'Pasaporte' }
	];

	// Resto del código permanece igual...
	nextStep(): void {
		if (this.currentStep() === 0) {
			const branchControls = ['branchName', 'branchAddress', 'branchPhone'];
			branchControls.forEach(control => {
				this.branchForm.get(control)?.markAsTouched();
			});

			if (this.branchForm.get('branchName')?.invalid ||
				this.branchForm.get('branchAddress')?.invalid ||
				this.branchForm.get('branchPhone')?.invalid) {
				return;
			}
		}

		this.currentStep.set(this.currentStep() + 1);
	}

	prevStep(): void {
		this.currentStep.set(this.currentStep() - 1);
	}

	isStepValid(step: number): boolean {
		if (step === 0) {
			return !this.branchForm.get('branchName')?.invalid &&
				!this.branchForm.get('branchAddress')?.invalid &&
				!this.branchForm.get('branchPhone')?.invalid;
		}
		return true;
	}

	onSubmit(): void {
		this.wasSubmitted = true;

		if (this.branchForm.invalid || this.isSubmit()) {
			this.branchForm.markAllAsTouched();
			return;
		}

		this.isSubmit.set(true);

		const formData = this.branchForm.getRawValue();

		// Simular envío a API
		setTimeout(() => {
			Swal.fire({
				icon: 'success',
				title: '¡Sucursal Creada!',
				text: `La sucursal "${formData.branchName}" ha sido creada exitosamente`,
				confirmButtonText: 'Aceptar'
			}).then(() => {
				this.router.navigate(['/admin']);
			});

			this.isSubmit.set(false);
		}, 1500);
	}

	onCancel(): void {
		this.router.navigate(['/admin']);
	}
}
