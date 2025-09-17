import { Routes } from '@angular/router';
import { AreaManagerDashboardComponent } from './area-manager-dashboard/area-manager-dashboard.component';
import { AreaManagerScreenComponent } from './area-manager-screen/area-manager-screen.component';
import { AreaManagerProfileComponent } from './Forms/area-manager-profile/area-manager-profile.component';
import { AreaManagerZoneComponent } from './Forms/area-manager-zone/area-manager-zone.component';
import { InventoryBaseComponent } from './Inventory/inventory-base/inventory-base.component';


export const AREA_MANAGER_ROUTES: Routes = [
	{
		path: '',
		component: AreaManagerScreenComponent,
		children: [
			{ path: 'dashboard', component:  AreaManagerDashboardComponent},
			{ path: 'profile', component:  AreaManagerProfileComponent},
			{ path: 'zone', component:  AreaManagerZoneComponent},
			{ path: 'inventory-base', component:  InventoryBaseComponent},
			{ path: '', redirectTo: 'dashboard', pathMatch: 'full' }
		]
	}
];
