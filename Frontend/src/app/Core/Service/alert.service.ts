import { Injectable } from '@angular/core';
import Swal, { SweetAlertResult } from 'sweetalert2';

@Injectable({
	providedIn: 'root'
})
export class AlertService {

	showLoading(title: string = 'Procesando...', text: string = 'Por favor espera un momento ⏳'): void {
		Swal.fire({
			title,
			text,
			allowOutsideClick: false,
			didOpen: () => {
				Swal.showLoading();
			}
		});
	}

	closeAlert(): void {
		Swal.close();
	}

	async withLoading(
		action: () => Promise<any>,
		options: {
			successTitle?: string;
			successText?: string;
			errorTitle?: string;
			errorText?: string;
			loadingTitle?: string;
			loadingText?: string;
		} = {}
	): Promise<SweetAlertResult<any>> {
		const {
			loadingTitle = 'Procesando...',
			loadingText = 'Por favor espera un momento ⏳',
			successTitle = '¡Éxito!',
			successText = 'Operación completada correctamente',
			errorTitle = 'Error',
			errorText = 'Ocurrió un error inesperado'
		} = options;

		this.showLoading(loadingTitle, loadingText);

		try {
			const result = await action();

			Swal.close();

			if (successTitle || successText) {
				return Swal.fire({
					icon: 'success',
					title: successTitle,
					text: successText,
					confirmButtonText: 'Aceptar'
				});
			}

			return result;
		} catch (error: any) {
			Swal.close();

			let errorMessage = errorText;

			if (error?.error?.message) {
				errorMessage = error.error.message;
			} else if (error?.error?.error) {
				errorMessage = error.error.error;
				const mensajeLimpio = errorMessage.split(':')[1]?.trim();
				if (mensajeLimpio) {
					errorMessage = mensajeLimpio;
				}
			} else if (error?.message) {
				errorMessage = error.message;
			}

			await Swal.fire({
				icon: 'error',
				title: errorTitle,
				text: errorMessage,
				confirmButtonText: 'Aceptar'
			});

			throw error;
		}
	}

	async confirmWithLoading(
		question: string,
		action: () => Promise<any>,
		options: {
			confirmText?: string;
			cancelText?: string;
			successTitle?: string;
			successText?: string;
			errorTitle?: string;
			errorText?: string;
		} = {}
	): Promise<void> {
		const {
			confirmText = 'Sí, continuar',
			cancelText = 'Cancelar',
			successTitle = '¡Éxito!',
			successText = 'Operación completada correctamente',
			errorTitle = 'Error',
			errorText = 'Ocurrió un error inesperado'
		} = options;

		const result = await Swal.fire({
			title: '¿Confirmar acción?',
			text: question,
			icon: 'question',
			showCancelButton: true,
			confirmButtonText: confirmText,
			cancelButtonText: cancelText
		});

		if (result.isConfirmed) {
			await this.withLoading(action, {
				loadingTitle: 'Procesando...',
				loadingText: 'Por favor espera un momento ⏳',
				successTitle,
				successText,
				errorTitle,
				errorText
			});
		}
	}
}
