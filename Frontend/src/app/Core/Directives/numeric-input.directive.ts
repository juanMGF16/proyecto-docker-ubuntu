// ==================================================
// Directiva: NumericInputDirective
// ==================================================
// Esta directiva restringe la entrada de texto a valores numéricos únicamente.
// También limita el número de caracteres según el valor definido en `maxLength`.

import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
	selector: '[appNumericInput]',
	standalone: true
})
export class NumericInputDirective {

	// Input principal de la directiva: cantidad máxima de caracteres permitidos
	@Input() maxLength: number = Infinity;

	// Constructor para obtener referencia directa al elemento del DOM
	constructor(private el: ElementRef<HTMLInputElement>) { }

	// Evento host: ejecutado cada vez que cambia el valor del input
	@HostListener('input', ['$event'])
	onInput(event: Event): void {
		this.formatValue(event.target as HTMLInputElement);
	}

	// Evento host: ejecutado cuando se pega texto en el input
	@HostListener('paste', ['$event'])
	onPaste(event: ClipboardEvent): void {
		event.preventDefault();

		const input = event.target as HTMLInputElement;
		const paste = event.clipboardData?.getData('text') || '';

		// Permitir solo caracteres numéricos
		let value = paste.replace(/\D/g, '');

		// Aplicar límite de longitud
		if (value.length > this.maxLength) {
			value = value.substring(0, this.maxLength);
		}

		input.value = value;
		input.dispatchEvent(new Event('input', { bubbles: true }));
	}

	// Método privado para dar formato y validar el valor ingresado
	private formatValue(input: HTMLInputElement): void {
		let value = input.value.replace(/\D/g, '');
		if (value.length > this.maxLength) {
			value = value.substring(0, this.maxLength);
		}
		input.value = value;
		input.dispatchEvent(new Event('input', { bubbles: true }));
	}
}
