import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleChange, MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router } from '@angular/router';
import { PersonMod } from '../../../Core/Models/SecurityModule/PersonMod.model';
import { AuthService } from '../../../Core/Service/Auth/auth.service';
import { colombianPhoneValidator, emailValidator } from '../../../Core/Utils/input-validators.util';
import { ColombianPhoneDirective } from '../../../Core/Directives/colombian-phone.directive';
import { OnlyNumbersDirective } from '../../../Core/Directives/only-numbers.directive';



@Component({
	selector: 'app-form-person',
	standalone: true,
	imports: [
		CommonModule,
		ReactiveFormsModule,
		MatFormFieldModule,
		MatInputModule,
		MatSelectModule,
		MatSlideToggleModule,
		MatButtonModule,
		MatIconModule,
		MatTooltipModule,
		OnlyNumbersDirective,
		ColombianPhoneDirective,
	],
	templateUrl: './form-person.component.html',
	styleUrls: ['./form-person.component.css']
})
export class FormPersonComponent implements OnInit, OnChanges {

	@Input() person: PersonMod | null = null;
	@Input() cancelRoute: string = '/Person';
	@Output() save = new EventEmitter<PersonMod>();

	formPerson!: FormGroup;
	isEditMode = false;
	showReactivarToggle = false;
	reactivarUsuario = false;

	private authService = inject(AuthService);
	private fb = inject(FormBuilder);
	private router = inject(Router);

	documentTypes = [
		{ value: 'RC', label: 'Registro Civil' },
		{ value: 'TI', label: 'Tarjeta de Identidad' },
		{ value: 'CC', label: 'Cédula de Ciudadanía' },
		{ value: 'CE', label: 'Cédula de Extranjería' },
		{ value: 'NIT', label: 'N° Identificación Tributaria' },
		{ value: 'PP', label: 'Pasaporte' }
	];

	ngOnInit(): void {
		this.buildForm();
	}

	ngOnChanges(changes: SimpleChanges): void {
		if (changes['person'] && this.person) {
			this.isEditMode = !!this.person.id;
			this.patchFormValues(this.person);
		}

		if (this.isEditMode && this.person) {
			const isInactive = this.person.active === false;
			const isAdmin = this.authService.getRole() === 'SM_ACTION';
			this.showReactivarToggle = isInactive && isAdmin;
		}
	}

	onToggleChange(event: MatSlideToggleChange): void {
		this.reactivarUsuario = event.checked;
		this.formPerson.patchValue({
			active: event.checked
		});
	}

	private buildForm(): void {
		this.formPerson = this.fb.group({
			name: ['', [Validators.required, Validators.minLength(3)]],
			lastName: ['', [Validators.required, Validators.minLength(3)]],
			email: ['', [Validators.required, emailValidator()]], // ← Quité Validators.email
			documentType: ['', Validators.required],
			documentNumber: ['', [
				Validators.required,
				Validators.pattern(/^[0-9]{6,10}$/),
			]],
			phone: ['', [
				Validators.required,
				colombianPhoneValidator()
			]],
			active: [true]
		});
	}

	private patchFormValues(person: PersonMod): void {
		const documentTypeTrimmed = person.documentType?.trim();

		this.formPerson.patchValue({
			...person,
			documentType: this.documentTypes.find(d => d.value === documentTypeTrimmed)?.value || '',
		});
	}

	onSubmit(): void {
		if (this.formPerson.invalid) {
			this.formPerson.markAllAsTouched();
			return;
		}

		const result: PersonMod = {
			...this.person,
			...this.formPerson.value,
			email: this.formPerson.value.email.trim().toLowerCase()
		};

		this.save.emit(result);
	}

	onCancel(): void {
		this.router.navigate([this.cancelRoute]);
	}
}
