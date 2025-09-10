import { Directive, HostListener, ElementRef } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
	selector: '[nitFormatter]',
	standalone: true
})
export class NitFormatterDirective {
	constructor(
		private el: ElementRef,
		private ngControl: NgControl
	) { }

	@HostListener('input', ['$event'])
	onInput(event: Event): void {
		const input = event.target as HTMLInputElement;
		let value = input.value.replace(/\D/g, ''); // Solo números

		// Limitar exactamente a 10 dígitos (9 + 1 verificador)
		if (value.length > 10) {
			value = value.substring(0, 10);
		}

		// Formatear con guión cuando tenga exactamente 10 dígitos
		if (value.length === 10) {
			value = value.substring(0, 9) + '-' + value.substring(9, 10);
		}

		// Actualizar el valor en el control del formulario
		if (this.ngControl.control) {
			this.ngControl.control.setValue(value);
		}

		// Actualizar el valor visual del input
		input.value = value;
	}

	@HostListener('blur', ['$event'])
	onBlur(event: Event): void {
		const input = event.target as HTMLInputElement;
		const value = input.value;

		// Validar formato final: exactamente 9 dígitos + guión + 1 dígito
		const nitPattern = /^\d{9}-\d{1}$/;

		if (value && !nitPattern.test(value)) {
			// Establecer error personalizado
			if (this.ngControl.control) {
				this.ngControl.control.setErrors({ invalidNIT: true });
			}
		} else if (value && nitPattern.test(value)) {
			// Limpiar errores de NIT si el formato es correcto
			if (this.ngControl.control && this.ngControl.control.errors) {
				delete this.ngControl.control.errors['invalidNIT'];

				// Si no hay otros errores, limpiar completamente
				if (Object.keys(this.ngControl.control.errors).length === 0) {
					this.ngControl.control.setErrors(null);
				}
			}
		}
	}
}
