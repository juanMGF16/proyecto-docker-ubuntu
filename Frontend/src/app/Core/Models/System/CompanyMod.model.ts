export interface CompanyOptionsMod {
	name: string;
	businessName: string;
	nit: string;
	industryId: number;
	email: string;
	website?: string;
	userId: number;
}

export interface CompanyMod {
	id: number;
	name: string;
	businessName: string;
	nit: string;
	industryId: string;
	industryName: string,
	email: string;
	website?: string;
	userId: number;
	state?: boolean;
}

export interface CompanyPartialUpdateMod {
	id: number;
	email?: string;
	website?: string;
}
