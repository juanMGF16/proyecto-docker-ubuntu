import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { BaseTableComponent } from '../../../../Components/SecurityModule/Base/base-table/base-table.component';
import { PersonMod } from '../../../../Core/Models/SecurityModule/PersonMod.model';
import { PersonService } from '../../../../Core/Service/SecurityModule/person.service';

@Component({
  selector: 'app-indice-person',
  standalone: true,
  imports: [MatCardModule, BaseTableComponent, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './indice-person.component.html',
  styleUrl: './indice-person.component.css'
})
export class IndicePersonComponent implements OnInit {

  personService = inject(PersonService);
  router = inject(Router)

  personData : PersonMod[] = [];
  columnasMostrar : string[] = [
    'N°', 'name', 'lastName', 'email', 'documentType', 'documentNumber', 'phone', 'active'
  ];

  ngOnInit(): void {
    this.cargarPersons();
  }

  cargarPersons(): void {
    this.personService.getAllJWT().subscribe({
      next: (data) => {
        this.personData = data;
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

  eliminarPerson(person: PersonMod): void {
    Swal.fire({
      title: '¿Qué tipo de eliminación deseas?',
      text: `Person: ${person.name}`,
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
        this.personService.delete(person.id, 0).subscribe(() => {
          Swal.fire('Eliminacion Logica ✅', '', 'success');
          this.cargarPersons();
        });
      } else if (result.isDenied) {
        this.personService.delete(person.id, 1).subscribe(() => {
          Swal.fire('Eliminacion Permanente ✅', '', 'success');
          this.cargarPersons();
        });
      }
    });
  }

  editarPerson(person: PersonMod): void {
    this.router.navigate(['/securitymodule/Person/Update', person.id]);
  }
}
