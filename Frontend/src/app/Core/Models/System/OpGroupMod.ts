export interface OpGroupOptionsMod {
	name: string;
	dateStart: string;
	dateEnd: string;
	areaManagerId: string;
}

export interface OpGroupMod {
	name: string;
	dateStart: string;
	dateEnd: string;
	areaManagerId: string;
	areaManagerName: string;
}

export interface OpGroupByAreaManagerMod {
	id: number;
	name: string;
}
