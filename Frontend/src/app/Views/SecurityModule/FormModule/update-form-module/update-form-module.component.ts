import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';
import { BaseFormPivoteComponent } from '../../../../Components/SecurityModule/Base/base-form-pivote/base-form-pivote.component';
import { FormModuleMod } from '../../../../Core/Models/SecurityModule/FormModuleMod.model';
import { FormModuleService } from '../../../../Core/Service/SecurityModule/form-module.service';
import { FormService } from '../../../../Core/Service/SecurityModule/form.service';
import { ModuleService } from '../../../../Core/Service/SecurityModule/module.service';

@Component({
  selector: 'app-update-form-module',
  standalone: true,
  imports: [BaseFormPivoteComponent, CommonModule],
  templateUrl: './update-form-module.component.html',
  styleUrl: './update-form-module.component.css'
})
export class UpdateFormModuleComponent implements OnInit {

  private route = inject(ActivatedRoute);
  private formService = inject(FormService);
  private moduleService = inject(ModuleService);
  private router = inject(Router);
  private formModuleService = inject(FormModuleService);

  entity!: FormModuleMod;
  selectFields: any[] = [];

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) {
      this.router.navigate(['/securitymodule/FormModule']);
      return;
    }

    this.formModuleService.getById(id).subscribe({
      next: (data) => {
        this.entity = data;
        this.loadSelects();
      },
      error: (err) => {
        console.log('Error al Obtener FormModule:', err);
        const mensajeCompleto = err?.error?.message || 'Ocurrio un error inesperado.';
        const mensaje = mensajeCompleto.split(':')[1]?.trim() || mensajeCompleto;
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: mensaje,
          confirmButtonText: 'Cerrar'
        })
      }
    });
  }

  private loadSelects(): void {
    forkJoin({
      forms: this.formService.getAll(),
      modules: this.moduleService.getAll()
    }).subscribe({
      next: ({ forms, modules }) => {
        this.selectFields = [
          {
            label: 'Form',
            controlName: 'formId',
            options: forms.map(form => ({ id: form.id, name: form.name }))
          },
          {
            label: 'Module',
            controlName: 'moduleId',
            options: modules.map(module => ({ id: module.id, name: module.name }))
          }
        ];
      },
      error: (err) => {
        console.error('Error al cargar usuarios o modules:', err);
      }
    });
  }

  handleUpdateFormModule(data: any): void {
    this.formModuleService.update(data).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Usuer-Module actualizado ✅',
          confirmButtonText: 'Aceptar'
        });
        this.router.navigate(['/securitymodule/FormModule']);
      },
      error: (err) => {
        console.log('Error al actualizar FormModule:', err);
        const mensajeCompleto = err?.error?.message || 'Ocurrio un error inesperado.';
        const mensaje = mensajeCompleto.split(':')[1]?.trim() || mensajeCompleto;
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: mensaje,
          confirmButtonText: 'Cerrar'
        })
      }
    });
  }
}
