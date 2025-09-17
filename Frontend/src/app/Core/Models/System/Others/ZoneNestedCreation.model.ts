export interface ZoneCreateRequestDTO {
	// Datos de la Zona
	zoneName: string;
	zoneDescription: string;
	branchId: number | null;

	// Datos del Encargado de Zona (Person)
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

