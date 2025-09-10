import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { BaseTableComponent } from '../../../../Components/SecurityModule/Base/base-table/base-table.component';
import { FormMod } from '../../../../Core/Models/SecurityModule/FormMod.model';
import { FormService } from '../../../../Core/Service/SecurityModule/form.service';

@Component({
  selector: 'app-indice-form',
  standalone: true,
  imports: [MatCardModule, BaseTableComponent, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './indice-form.component.html',
  styleUrl: './indice-form.component.css'
})
export class IndiceFormComponent implements OnInit {

  formService = inject(FormService);
  router = inject(Router)

  formData : FormMod[] = [];
  columnasMostrar : string[] = [
    'N°', 'name', 'description', 'active'
  ];

  ngOnInit(): void {
    this.cargarForms();
  }

  cargarForms(): void {
    this.formService.getAllJWT().subscribe({
      next: (data) => {
        this.formData = data;
        // console.log(data);
      },
      error: (err) => {
        console.log('Error al cargar los datos:', err);
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

  eliminarForm(form: FormMod): void {
    Swal.fire({
      title: '¿Qué tipo de eliminación deseas?',
      text: `Form: ${form.name}`,
      icon: 'warning',
      showCancelButton: true,
      showDenyButton: true,
      confirmButtonText: 'Lógica',
      denyButtonText: 'Permanente',
      cancelButtonText: 'Cancelar',
      confirmButtonColor: '#3085d6',
      denyButtonColor: '#d33',
    }).then(result => {
      if (result.isConfirmed) {
        this.formService.delete(form.id, 0).subscribe(() => {
          Swal.fire('Eliminacion Logica ✅', '', 'success');
          this.cargarForms();
        });
      } else if (result.isDenied) {
        this.formService.delete(form.id, 1).subscribe(() => {
          Swal.fire('Eliminacion Permanente ✅', '', 'success');
          this.cargarForms();
        });
      }
    });
  }

  editarForm(form: FormMod): void {
    this.router.navigate([`/securitymodule/Form/Update/${form.id}`]);
  }
}
