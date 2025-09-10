import { ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';

export function mixedPhoneValidator(): ValidatorFn {
	return (control: AbstractControl): ValidationErrors | null => {
		const value = control.value;

		if (!value) {
			return null;
		}

		// Eliminar cualquier carácter que no sea número
		const numericValue = value.replace(/\D/g, '');

		// Validar longitud (10 dígitos)
		if (numericValue.length !== 10) {
			return { mixedPhone: true };
		}

		// Permitir celulares (empezando con 3) y fijos (empezando con 60, 4, 5, 6, 7, 8)
		const isValid = /^3\d{9}$/.test(numericValue) ||
			/^[4-8]\d{8}$/.test(numericValue) ||
			/^60\d{8}$/.test(numericValue);

		return isValid ? null : { mixedPhone: true };
	};
}
