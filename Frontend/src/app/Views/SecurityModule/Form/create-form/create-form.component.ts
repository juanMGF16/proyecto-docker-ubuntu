import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { BaseFormEntityComponent } from '../../../../Components/SecurityModule/Base/base-form-entity/base-form-entity.component';
import { FormMod } from '../../../../Core/Models/SecurityModule/FormMod.model';
import { FormService } from '../../../../Core/Service/SecurityModule/form.service';

@Component({
  selector: 'app-create-form',
  standalone: true,
  imports: [BaseFormEntityComponent],
  templateUrl: './create-form.component.html',
  styleUrl: './create-form.component.css'
})
export class CreateFormComponent {
  private formService = inject(FormService);
  private router = inject(Router);

  handleSaveForm(newForm: FormMod): void {
      // console.log(newForm)
      this.formService.create(newForm).subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: `Creacion de ${newForm.name} ✅`,
            confirmButtonText: 'Aceptar'
          })
          this.router.navigate(['/securitymodule/Form']);
        },
        error: (err) => {
          console.log('Error al crear Form:', err);
          const mensajeCompleto = err?.error?.message || 'Ocurrio un error inesperado.';
          const mensaje = mensajeCompleto.split(':')[1]?.trim() || mensajeCompleto;
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: mensaje,
            confirmButtonText: 'Aceptar'
          });;
        }
      });
    }
}
