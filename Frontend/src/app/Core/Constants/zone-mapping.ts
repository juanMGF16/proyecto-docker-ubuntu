export const ZONE_STATE_MAP: Record<string, string> = {
	Available: 'Disponible',
	InInventory: 'En Inventario',
	InVerification: 'En Verificación'
};

export const ZONE_STATE_ESPECIFIC_MAP: Record<
	'Available' | 'InInventory' | 'InVerification',
	{ label: string; cssClass: string; icon: string }
> = {
	Available: {
		label: 'Disponible',
		cssClass: 'state-disponible',
		icon: 'fiber_manual_record'
	},
	InInventory: {
		label: 'En Inventario',
		cssClass: 'state-en-inventario',
		icon: 'fiber_manual_record'
	},
	InVerification: {
		label: 'En Verificación',
		cssClass: 'state-en-verificación',
		icon: 'fiber_manual_record'
	}
};
