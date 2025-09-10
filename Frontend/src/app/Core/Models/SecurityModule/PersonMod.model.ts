export interface PersonMod{
    id : number,
    name : string,
    lastName : string,
    email : string,
    documentType : string,
    documentNumber : string,
    phone : string,
    active : boolean,
		lastLogin? : Date
}

export interface PersonAvailableMod{
    id : number,
    name : string
}
