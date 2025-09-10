import { Component, inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { BaseFormEntityComponent } from '../../../../Components/SecurityModule/Base/base-form-entity/base-form-entity.component';
import { FormMod } from '../../../../Core/Models/SecurityModule/FormMod.model';
import { FormService } from '../../../../Core/Service/SecurityModule/form.service';

@Component({
  selector: 'app-update-form',
  standalone: true,
  imports: [BaseFormEntityComponent],
  templateUrl: './update-form.component.html',
  styleUrl: './update-form.component.css'
})
export class UpdateFormComponent {

  private formService = inject(FormService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  form: FormMod | null = null;

  ngOnInit(): void {
    const formId = Number(this.route.snapshot.paramMap.get('id'));
    this.formService.getById(formId).subscribe({
      next: (data) => this.form = data,
      error: (err) => {
        console.log('Error al obtener Datos:', err);
        const mensajeCompleto = err?.error?.message || 'Ocurrio un error inesperado.';
        const mensaje = mensajeCompleto.split(':')[1]?.trim() || mensajeCompleto;
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: mensaje,
          confirmButtonText: 'Aceptar'
        });
      }
    });
  }

  handleSaveForm(updatedForm: FormMod): void {
    if (!updatedForm.id) return;
    // console.log(updatedForm)
    this.formService.update(updatedForm).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: `Actualizacion de ${updatedForm.name} ✅`,
          confirmButtonText: 'Aceptar'
        })
        this.router.navigate(['/securitymodule/Form']);
      },
      error: (err) => {
        console.log('Error al actualizar Form:', err);
        const mensajeCompleto = err?.error?.message || 'Ocurrio un error inesperado.';
        const mensaje = mensajeCompleto.split(':')[1]?.trim() || mensajeCompleto;
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: mensaje,
          confirmButtonText: 'Aceptar'
        });
      }
    });
  }
}
