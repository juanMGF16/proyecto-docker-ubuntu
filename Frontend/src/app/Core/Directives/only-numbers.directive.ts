import { Directive, HostListener, Input } from '@angular/core';

@Directive({
	selector: '[appOnlyNumbers]',
	standalone: true
})
export class OnlyNumbersDirective {
	@Input() maxLength: number = Infinity;

	@HostListener('keydown', ['$event'])
	onKeyDown(event: KeyboardEvent): void {
		const input = event.target as HTMLInputElement;
		const currentValue = input.value;
		const key = event.key;

		// Permitir teclas de control
		if (['Backspace', 'Tab', 'Enter', 'Escape', 'ArrowLeft', 'ArrowRight', 'Delete', 'Home', 'End'].includes(key)) {
			return;
		}

		// Permitir combinaciones con Ctrl o Cmd (copiar, pegar, cortar, seleccionar todo)
		if ((event.ctrlKey || event.metaKey) && ['a', 'c', 'v', 'x'].includes(key.toLowerCase())) {
			return;
		}

		// Solo permitir números
		if (!/^\d$/.test(key)) {
			event.preventDefault();
			return;
		}

		// Validar longitud máxima
		if (currentValue.length >= this.maxLength) {
			event.preventDefault();
		}
	}

	@HostListener('paste', ['$event'])
	onPaste(event: ClipboardEvent): void {
		event.preventDefault();

		const input = event.target as HTMLInputElement;
		const paste = event.clipboardData?.getData('text') || '';

		// Filtrar solo números
		let filtered = paste.replace(/\D/g, '');

		// Respetar maxLength
		const allowed = filtered.substring(0, this.maxLength - input.value.length);

		// Insertar en la posición actual del cursor
		const start = input.selectionStart ?? 0;
		const end = input.selectionEnd ?? 0;
		const newValue =
			input.value.substring(0, start) + allowed + input.value.substring(end);

		input.value = newValue;

		// Disparar evento input para que Angular reactive forms detecte el cambio
		input.dispatchEvent(new Event('input', { bubbles: true }));
	}
}
