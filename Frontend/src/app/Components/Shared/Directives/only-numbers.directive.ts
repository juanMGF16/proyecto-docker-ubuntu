// only-numbers.directive.ts (versión mejorada)
import { Directive, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[appOnlyNumbers]',
  standalone: true
})
export class OnlyNumbersDirective {
  @Input() maxLength: number = Infinity; // Longitud máxima opcional

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    const input = event.target as HTMLInputElement;
    const currentValue = input.value;
    const key = event.key;

    // Permitir teclas de control
    if (['Backspace', 'Tab', 'Enter', 'Escape', 'ArrowLeft', 'ArrowRight', 'Delete', 'Home', 'End'].includes(key)) {
      return;
    }

    // Solo permitir números
    if (!/^\d$/.test(key)) {
      event.preventDefault();
      return;
    }

    // Validar longitud máxima si se especificó
    if (currentValue.length >= this.maxLength) {
      event.preventDefault();
    }
  }

  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent): void {
    const paste = event.clipboardData?.getData('text') || '';

    // Validar que solo sean números
    if (!/^\d+$/.test(paste)) {
      event.preventDefault();
      return;
    }

    // Validar longitud máxima si se especificó
    if (paste.length > this.maxLength) {
      event.preventDefault();
    }
  }
}
