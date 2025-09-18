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

export interface UserPartialUpdate {
	id: number;
	username?: string;

	name?: string;
	lastName?: string;
	email?: string;
	phone?: string;
}
