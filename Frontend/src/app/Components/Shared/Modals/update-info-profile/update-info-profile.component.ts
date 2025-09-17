import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import Swal from 'sweetalert2';

export interface ProfileEditField {
  key: string;
  label: string;
  type?: string;
  validators?: any[];
  visible?: boolean;
}

@Component({
  selector: 'app-update-info-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './update-info-profile.component.html',
  styleUrls: [
    '../../Styles/modal-shared.css',
    './update-info-profile.component.css'
  ]
})
export class UpdateInfoProfileComponent implements OnInit, OnChanges {
  private readonly formBuilder = inject(FormBuilder);

  @Input({ required: true }) profileData: any;
  @Input({ required: true }) isOpen = false;
  @Input({ required: true }) editFields: ProfileEditField[] = [];
  @Input() saveService!: (data: any) => any;

  @Output() onClose = new EventEmitter<void>();
  @Output() onSave = new EventEmitter<any>();

  editForm!: FormGroup;
  isSaving = signal(false);

  ngOnInit(): void {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['profileData'] && this.editForm) {
      this.updateFormValues();
    }
  }

  private initForm(): void {
    const formGroupConfig: any = {};

    this.editFields.forEach(field => {
      if (field.visible !== false) {
        formGroupConfig[field.key] = [
          this.profileData?.[field.key] || '',
          field.validators || []
        ];
      }
    });

    this.editForm = this.formBuilder.group(formGroupConfig);
  }

  private updateFormValues(): void {
    if (this.profileData && this.editForm) {
      const formValues: any = {};

      this.editFields.forEach(field => {
        if (field.visible !== false) {
          formValues[field.key] = this.profileData[field.key] || '';
        }
      });

      this.editForm.patchValue(formValues);
    }
  }

  closeModal(): void {
    if (this.isSaving()) return;
    this.editForm.reset();
    this.onClose.emit();
  }

  saveChanges(): void {
    if (this.editForm.invalid) {
      this.markFormGroupTouched(this.editForm);
      return;
    }

    const formData = {
      ...this.editForm.getRawValue(),
      id: this.profileData?.id
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

        if (this.saveService) {
          this.saveService(formData).subscribe({
            next: (updatedData: any) => {
              this.isSaving.set(false);
              Swal.fire({
                title: '¡Éxito!',
                text: 'Perfil actualizado correctamente',
                icon: 'success',
                timer: 2000,
                showConfirmButton: false
              });
              this.onSave.emit(updatedData);
            },
            error: (error: any) => {
              this.isSaving.set(false);
              Swal.fire({
                title: 'Error',
                text: error.error?.message || 'No se pudo actualizar el perfil',
                icon: 'error',
                confirmButtonText: 'Entendido'
              });
            }
          });
        } else {
          this.isSaving.set(false);
          this.onSave.emit(formData);
        }
      }
    });
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }

  getFieldErrors(fieldKey: string): string[] {
    const control = this.editForm.get(fieldKey);
    const errors: string[] = [];

    if (control?.errors && control.touched) {
      if (control.errors['required']) errors.push('Este campo es requerido');
      if (control.errors['minlength']) errors.push(`Mínimo ${control.errors['minlength'].requiredLength} caracteres`);
      if (control.errors['maxlength']) errors.push(`Máximo ${control.errors['maxlength'].requiredLength} caracteres`);
      if (control.errors['email']) errors.push('Formato de email inválido');
    }

    return errors;
  }
}
