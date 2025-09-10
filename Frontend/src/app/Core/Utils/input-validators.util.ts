import { AbstractControl, ValidatorFn } from '@angular/forms';

/**
 * Bloquea cualquier intento de pegar texto no numérico en un input.
 * Ahora con soporte para formato colombiano
 */
export function blockNonNumericPaste(event: ClipboardEvent, maxLength: number = 10, mustStartWith: string = ''): void {
	const paste = event.clipboardData?.getData('text') || '';

	// Validar que solo sean números
	if (!/^\d+$/.test(paste)) {
		event.preventDefault();
		return;
	}

	// Validar longitud máxima
	if (paste.length > maxLength) {
		event.preventDefault();
		return;
	}

	// Validar si debe empezar con un carácter específico
	if (mustStartWith && !paste.startsWith(mustStartWith)) {
		event.preventDefault();
	}
}

/**
 * Solo permite ingreso de caracteres numéricos mediante teclado.
 * Ahora con soporte para formato colombiano
 */
export function onlyNumberInput(event: KeyboardEvent, currentValue: string, maxLength: number = 10, mustStartWith: string = ''): void {
	const charCode = event.key;

	// Permitir teclas de control
	if (['Backspace', 'Tab', 'Enter', 'Escape', 'ArrowLeft', 'ArrowRight', 'Delete', 'Home', 'End'].includes(charCode)) {
		return;
	}

	// Solo permitir números
	if (!/^\d$/.test(charCode)) {
		event.preventDefault();
		return;
	}

	// Validar longitud máxima
	if (currentValue.length >= maxLength) {
		event.preventDefault();
		return;
	}

	// Validar si debe empezar con un carácter específico (para primer dígito)
	if (currentValue.length === 0 && mustStartWith && charCode !== mustStartWith) {
		event.preventDefault();
	}
}

/**
 * Validador para números de celular colombiano
 */
export function colombianPhoneValidator(): ValidatorFn {
	return (control: AbstractControl): { [key: string]: any } | null => {
		const value = control.value;
		if (!value) return null;

		// Validar que solo contenga números
		if (!/^\d+$/.test(value)) {
			return { colombianPhone: 'Solo se permiten números' };
		}

		// Validar que empiece con 3
		if (!value.startsWith('3')) {
			return { colombianPhone: 'El número debe empezar con 3' };
		}

		// Validar longitud exacta de 10 dígitos
		if (value.length !== 10) {
			return { colombianPhone: 'Debe tener exactamente 10 dígitos' };
		}

		return null; // Válido
	};
}

/**
 * Solo permite contraseñas con el formato indicado.
 */
export function strongPassword(): ValidatorFn {
	return (control: AbstractControl): { [key: string]: any } | null => {
		const value = control.value;
		if (!value) return null;

		const hasUpperCase = /[A-Z]/.test(value);
		const hasLowerCase = /[a-z]/.test(value);
		const hasNumber = /[0-9]/.test(value);
		const hasSpecialChar = /[!@#$%^_&*+(),.?":{}|<>]/.test(value);

		const valid = hasUpperCase && hasLowerCase && hasNumber && hasSpecialChar;

		return !valid ? { strongPassword: true } : null;
	};
}

/**
 * Validador para formato de email básico pero más completo
 */
export function emailValidator(): ValidatorFn {
	return (control: AbstractControl): { [key: string]: any } | null => {
		const value = control.value;
		if (!value) return null; // Permitir campo vacío (required se encarga)

		// Expresión regular más flexible pero aún válida
		const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

		if (!emailPattern.test(value)) {
			return { emailFormat: true };
		}

		return null; // Válido
	};
}
