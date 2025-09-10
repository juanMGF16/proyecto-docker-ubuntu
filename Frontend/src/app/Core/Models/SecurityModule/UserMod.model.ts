export interface UserMod {
	id: number,
	username: string,
	password: string,
	active: boolean,

	personId: number,
	personName: string
}

export interface UserOptionsMod {
	id: number,
	username: string,
	password?: string,
	active: boolean

	personId: number,
}

export interface UserPartialUpdateMod {
	id: number;
	username?: string;

	email?: string;
	phone?: string;
}

export interface UserHasCompanyMod {
  hasCompany: boolean;
  companyId: number | null;
}

