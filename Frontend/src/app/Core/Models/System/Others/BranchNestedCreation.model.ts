export interface BranchCreateRequestDTO {
  // Datos de la Sucursal
  branchName: string;
  branchAddress: string;
  branchPhone: string;
  companyId: number | null;

  // Datos del Subadministrador (Person)
  personName: string;
  personLastName: string;
  personEmail: string;
  personDocumentType: string;
  personDocumentNumber: string;
  personPhone: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  field?: string;
}

