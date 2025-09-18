import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
	selector: '[appNumericInput]',
	standalone: true
})
export class NumericInputDirective {
	@Input() maxLength: number = Infinity;

	constructor(private el: ElementRef<HTMLInputElement>) { }

	@HostListener('input', ['$event'])
	onInput(event: Event): void {
		this.formatValue(event.target as HTMLInputElement);
	}

	@HostListener('paste', ['$event'])
	onPaste(event: ClipboardEvent): void {
		event.preventDefault();

		const input = event.target as HTMLInputElement;
		const paste = event.clipboardData?.getData('text') || '';

		// Solo números
		let value = paste.replace(/\D/g, '');

		// Limitar a maxLength
		if (value.length > this.maxLength) {
			value = value.substring(0, this.maxLength);
		}

		input.value = value;
		input.dispatchEvent(new Event('input', { bubbles: true }));
	}

	private formatValue(input: HTMLInputElement): void {
		let value = input.value.replace(/\D/g, '');
		if (value.length > this.maxLength) {
			value = value.substring(0, this.maxLength);
		}
		input.value = value;
		input.dispatchEvent(new Event('input', { bubbles: true }));
	}
}
