import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Validadores para numeros
 */
export function colombianPhoneValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value as string;
    if (!value) return null;

    if (!/^\d+$/.test(value)) {
      return { colombianPhone: 'Solo números permitidos' };
    }

    if (value.length !== 10) {
      return { colombianPhone: 'Debe tener 10 dígitos' };
    }

    if (!value.startsWith('3')) {
      return { colombianPhone: 'Debe comenzar con 3' };
    }

    return null;
  };
}

export function documentNumberValidator(min: number, max: number): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value as string;
    if (!value) return null;

    if (!/^\d+$/.test(value)) {
      return { documentNumber: 'Solo números permitidos' };
    }
    if (value.length < min || value.length > max) {
      return { documentNumber: `Debe tener entre ${min} y ${max} dígitos` };
    }
    return null;
  };
}

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
