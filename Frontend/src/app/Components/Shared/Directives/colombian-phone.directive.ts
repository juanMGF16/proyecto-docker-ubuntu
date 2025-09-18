// colombian-phone.directive.ts
import { Directive, HostListener, Input, ElementRef } from '@angular/core';
import { onlyNumberInput, blockNonNumericPaste } from '../../../Core/Utils/input-validators.util';

@Directive({
	selector: '[appColombianPhone]',
	standalone: true
})
export class ColombianPhoneDirective {
	@Input() maxLength: number = 10;
	@Input() mustStartWith: string = '3';

	constructor(private el: ElementRef) { }

	@HostListener('keydown', ['$event'])
	onKeyDown(event: KeyboardEvent): void {
		const currentValue = this.el.nativeElement.value;
		onlyNumberInput(event, currentValue, this.maxLength, this.mustStartWith);
	}

	@HostListener('paste', ['$event'])
	onPaste(event: ClipboardEvent): void {
		blockNonNumericPaste(event, this.maxLength, this.mustStartWith);
	}

	@HostListener('input', ['$event'])
	onInput(event: Event): void {
		const input = event.target as HTMLInputElement;
		let value = input.value;

		// Remover caracteres no numéricos
		value = value.replace(/\D/g, '');

		// Limitar a la longitud máxima
		if (value.length > this.maxLength) {
			value = value.substring(0, this.maxLength);
		}

		// Asegurar que empiece con el carácter requerido
		if (value.length > 0 && this.mustStartWith && !value.startsWith(this.mustStartWith)) {
			value = this.mustStartWith + value.substring(1);
		}

		input.value = value;

		// Disparar evento de input para Angular Forms
		input.dispatchEvent(new Event('input'));
	}
}
