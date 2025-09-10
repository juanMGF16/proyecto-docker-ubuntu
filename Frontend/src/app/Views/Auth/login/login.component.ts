import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { InitialHeaderComponent} from "../../../Components/System/Landing/initial-header/initial-navbar.component";
import { AuthService } from '../../../Core/Service/Auth/auth.service';
import { RoleRedirectService } from '../../../Core/Service/Auth/role-redirect.service';

@Component({
	selector: 'app-login',
	standalone: true,
	imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, RouterLink, MatIconModule, InitialHeaderComponent],
	templateUrl: './login.component.html',
	styleUrl: './login.component.css'
})
export class LoginComponent {

	private formBuilder = inject(FormBuilder);
	private authService = inject(AuthService);
	private roleRedirect = inject(RoleRedirectService);

	hidePassword = true;
	usernameFocused = false;
	passwordFocused = false;

	loginForm = this.formBuilder.group({
		username: ['', Validators.required],
		password: ['', [
			Validators.required
		]]
	});

	onSubmit(): void {
		if (this.loginForm.invalid) {
			this.loginForm.markAllAsTouched();
			return;
		}

		const { username, password } = this.loginForm.value;

		this.authService.login({ username: username!, password: password! }).subscribe({
			next: (res) => {
				this.authService.saveToken(res.token);

				// Delega la decisión al servicio
				const role = this.authService.getRole();
				this.roleRedirect.redirectUser(role);
			},
			error: () => {
				Swal.fire({
					icon: 'error',
					title: 'Oopss...',
					text: 'Credenciales Incorrectas',
					confirmButtonText: 'Aceptar'
				});
			}
		});
	}

}
