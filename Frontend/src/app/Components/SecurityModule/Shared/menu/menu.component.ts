import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import {MatMenuModule} from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink } from '@angular/router';

import Swal from 'sweetalert2';
import { AuthService } from '../../../../Core/Service/Auth/auth.service';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, RouterLink, MatMenuModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent {

  private authService = inject(AuthService);

  logout() {
    Swal.fire({
      title: "¿Deseas cerrar sesión?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: "Sí, Cerrar Sesión",
      confirmButtonColor: "#d33",
      cancelButtonText: "No, Quedarme"
    }).then((result) => {
      if (result.isConfirmed) {
        this.authService.logout();
        Swal.fire("Sesión cerrada", "", "success");
      }
    });
  }
}
