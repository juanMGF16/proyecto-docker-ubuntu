import Swal, { SweetAlertResult } from 'sweetalert2';

export function confirmLogout(): Promise<SweetAlertResult<any>> {
  return Swal.fire({
    title: "¿Deseas cerrar sesión?",
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: "Sí, Cerrar Sesión",
    confirmButtonColor: "#d33",
    cancelButtonText: "No, Quedarme"
  });
}

export function successMessage(title: string, text: string = "") {
  return Swal.fire(title, text, "success");
}
