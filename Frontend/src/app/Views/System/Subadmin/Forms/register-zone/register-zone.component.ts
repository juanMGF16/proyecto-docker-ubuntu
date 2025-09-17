import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { UserService } from '../../../../../Core/Service/SecurityModule/user.service';
import { BranchService } from '../../../../../Core/Service/System/branch.service';
import { colombianPhoneValidator, documentNumberValidator, emailValidator, mixedPhoneValidator } from '../../../../../Core/Utils/input-validators.util';
import { NumericInputDirective } from '../../../../../Core/Directives/numeric-input.directive';
import { AdminNavService } from '../../../../../Core/Service/Navigation/admin-nav.service';
import { AuthService } from '../../../../../Core/Service/Auth/auth.service';
import { catchError, of } from 'rxjs';
import { ZoneCreateRequestDTO } from '../../../../../Core/Models/System/Others/ZoneNestedCreation.model';
import { SubadminNavService } from '../../../../../Core/Service/Navigation/subadmin-nav.service';
import { ZoneService } from '../../../../../Core/Service/System/zone.service';

@Component({
	selector: 'app-register-zone',
	standalone: true,
	imports: [
		CommonModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatIconModule,
		MatStepperModule,
		NumericInputDirective
	],
	templateUrl: './register-zone.component.html',
	styleUrls: ['../../../../../Components/Shared/Styles/register-nested-shared.css', './register-zone.component.css']
})
export class RegisterZoneComponent implements OnInit {
	private formBuilder = inject(FormBuilder);
	private router = inject(Router);
	private authService = inject(AuthService)
	private branchService = inject(BranchService);
	private zoneService = inject(ZoneService);
	private navService = inject(SubadminNavService);

	isSubmit = signal(false);
	wasSubmitted = false;
	currentStep = signal(0);
	branchId: number | null = null;

	branchForm = this.formBuilder.nonNullable.group({
		// Sección Zone
		zoneName: ['', [Validators.required, Validators.minLength(3)]],
		zoneDescription: ['', [Validators.minLength(5)]],

		// Sección Encargado de Zona
		encZoneName: ['', [Validators.required, Validators.minLength(3)]],
		encZoneLastName: ['', [Validators.required, Validators.minLength(3)]],
		encZoneDocumentType: ['', Validators.required],
		encZoneDocumentNumber: ['', [Validators.required, documentNumberValidator(6, 10)]],
		encZonePhone: ['', [Validators.required, colombianPhoneValidator()]],
		encZoneEmail: ['', [Validators.required, emailValidator()]]
	});

	documentTypes = [
		{ value: 'RC', label: 'Registro Civil' },
		{ value: 'TI', label: 'Tarjeta de Identidad' },
		{ value: 'CC', label: 'Cédula de Ciudadanía' },
		{ value: 'CE', label: 'Cédula de Extranjería' },
		{ value: 'PP', label: 'Pasaporte' }
	];

	nextStep(): void {
		if (this.currentStep() === 0) {
			const branchControls = ['zoneName', 'zoneDescription'];
			branchControls.forEach(control => {
				this.branchForm.get(control)?.markAsTouched();
			});

			if (this.branchForm.get('zoneName')?.invalid ||
				this.branchForm.get('zoneDescription')?.invalid) {
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
			return !this.branchForm.get('zoneName')?.invalid &&
				!this.branchForm.get('zoneDescription')?.invalid
		}
		return true;
	}

	ngOnInit(): void {
		const userIdString = this.authService.getIdUser();
		const idUser = parseInt(userIdString, 10);
		if (isNaN(idUser)) {
			console.log('ID de usuario no válido');
			return;
		}

		this.branchService.getByIdInCharge(idUser).pipe(
			catchError(error => {
				console.log('Error al obtener la sucursal: ' + error.message);
				return of(null);
			})
		).subscribe(branch => {
			if (!branch) {
				console.log('No se pudo obtener la sucursal');
				return;
			}
			this.branchId = branch.id;
		});
	}

	onSubmit(): void {
		this.wasSubmitted = true;

		if (this.branchForm.invalid || this.isSubmit()) {
			this.branchForm.markAllAsTouched();
			return;
		}

		this.isSubmit.set(true);

		const formData = this.branchForm.getRawValue();

		// Mapear al DTO esperado por el backend
		const request: ZoneCreateRequestDTO = {
			zoneName: formData.zoneName,
			zoneDescription: formData.zoneDescription,
			branchId: this.branchId,

			personName: formData.encZoneName,
			personLastName: formData.encZoneLastName,
			personEmail: formData.encZoneEmail,
			personDocumentType: formData.encZoneDocumentType,
			personDocumentNumber: formData.encZoneDocumentNumber,
			personPhone: formData.encZonePhone
		};

		console.log(request)

		this.zoneService.createWithEncZone(request).subscribe({
			next: () => {
				Swal.fire({
					icon: 'success',
					title: '¡Sucursal Creada!',
					text: `La sucursal "${formData.zoneName}" ha sido creada exitosamente`,
					confirmButtonText: 'Aceptar'
				}).then(() => {
					this.navService.triggerRefreshZones();
					this.router.navigate(['/subadmin']);
				});

				this.isSubmit.set(false);
			},
			error: (err) => {
				console.error('Error API', err);
				if (err.status === 400 && err.error?.message) {
					Swal.fire({
						icon: 'warning',
						title: 'Error de validación',
						text: err.error.message,
						confirmButtonText: 'Aceptar'
					});
				} else {
					Swal.fire({
						icon: 'error',
						title: 'Error en el servidor',
						text: 'Ocurrió un error interno, por favor inténtalo nuevamente',
						confirmButtonText: 'Aceptar'
					});
				}

				this.isSubmit.set(false);
			}
		});

	}

	onCancel(): void {
		this.router.navigate(['/subadmin']);
	}
}
