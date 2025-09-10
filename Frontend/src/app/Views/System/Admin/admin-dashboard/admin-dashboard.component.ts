import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { KpiCardComponent } from '../../../../Components/System/Admin/Analytics/kpi-card/kpi-card.component';
import { CategoryChartComponent } from "../../../../Components/System/Admin/Analytics/category-chart/category-chart.component";
import { StatusChartComponent } from "../../../../Components/System/Admin/Analytics/status-chart/status-chart.component";
import { CompanyService } from '../../../../Core/Service/System/company.service';
import { UserService } from '../../../../Core/Service/SecurityModule/user.service';
import { DashboardModel } from '../../../../Core/Models/System/Others/Dashboard.model';

type Rol = 'subadmins' | 'encargados' | 'verificadores' | 'operativos';

@Component({
	selector: 'app-admin-dashboard',
	standalone: true,
	imports: [CommonModule, MatIconModule, KpiCardComponent, CategoryChartComponent, StatusChartComponent],
	templateUrl: './admin-dashboard.component.html',
	styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent {

	userService = inject(UserService);
	companyService = inject(CompanyService);

	// KPIs
	totalSucursales = 0;
	totalZonas = 0;
	totalItems = 0;

	// Usuarios por rol
	usuariosPorRol: Record<Rol, number> = {
		subadmins: 0,
		encargados: 0,
		verificadores: 0,
		operativos: 0
	};

	// Graficas
	itemsPorCategoria: Record<string, number> = {};
	itemsPorEstado: Record<string, number> = {};

	ngOnInit(): void {
		this.userService.hasCompany().subscribe({
			next: (res) => {
				if (res.hasCompany && res.companyId) {
					this.loadDashboard(res.companyId);
				}
			},
			error: (err) => console.error('Error obteniendo empresa del usuario:', err)
		});
	}

	private loadDashboard(companyId: number) {
		this.companyService.getDashboard(companyId).subscribe({
			next: (data: DashboardModel) => {
				console.log(data)
				this.totalSucursales = data.totalBranches;
				this.totalZonas = data.totalZones;
				this.totalItems = data.totalItems;

				// Mapear roles de API → roles internos del frontend
				this.usuariosPorRol = {
					subadmins: data.usersByRole['SUBADMINISTRADOR'] ?? 0,
					encargados: data.usersByRole['ENCARGADO_ZONA'] ?? 0,
					verificadores: data.usersByRole['VERIFICADOR'] ?? 0,
					operativos: data.usersByRole['OPERATIVO'] ?? 0
				};

				this.itemsPorCategoria = data.itemsByCategory;
				this.itemsPorEstado = data.itemsByState;
			},
			error: (err) => console.error('Error cargando dashboard:', err)
		});
	}

	get totalUsuarios(): number {
		return Object.values(this.usuariosPorRol).reduce((a, b) => a + b, 0);
	}


	getRoleKeys(): Rol[] {
		return Object.keys(this.usuariosPorRol) as Rol[];
	}

	getRoleIcon(role: Rol): string {
		const icons: Record<Rol, string> = {
			subadmins: 'admin_panel_settings',
			encargados: 'supervisor_account',
			verificadores: 'verified_user',
			operativos: 'engineering'
		};
		return icons[role];
	}

	getRoleLabel(role: Rol): string {
		const labels: Record<Rol, string> = {
			subadmins: 'Subadmins',
			encargados: 'Encargados',
			verificadores: 'Verificadores',
			operativos: 'Operativos'
		};
		return labels[role];
	}
}
