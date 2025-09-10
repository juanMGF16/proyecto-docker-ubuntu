export interface CompanyCreateDTO {
	name: string;
	businessName: string;
	nit: string;
	industry: number;
	email: string;
	website?: string;
	userId: number;
}

export interface CompanyConsultDTO {
	id: number;
	name: string;
	businessName: string;
	nit: string;
	industryId: string;
	industryName: string,
	email: string;
	website?: string;
	userId: number;
	createdAt?: Date;
	updatedAt?: Date;
	state?: boolean;
}
