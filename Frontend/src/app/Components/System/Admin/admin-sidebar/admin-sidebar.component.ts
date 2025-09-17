import { CommonModule } from '@angular/common';
import { Component, EventEmitter, HostListener, Input, Output, OnInit, OnDestroy, OnChanges, SimpleChanges, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Subscription } from 'rxjs';
import { AdminNavService, NavigationItem, NavigationState } from '../../../../Core/Service/Navigation/admin-nav.service';
import { BranchService } from '../../../../Core/Service/System/branch.service';
import { UserService } from '../../../../Core/Service/SecurityModule/user.service';
import { BranchByCompanyMod } from '../../../../Core/Models/System/BranchMod.model';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-admin-sidebar',
	imports: [CommonModule, MatIconModule, MatButtonModule],
	standalone: true,
	templateUrl: './admin-sidebar.component.html',
	styleUrls: ['../../../Shared/Styles/sidebar-shared.css','./admin-sidebar.component.css']
})
export class AdminSidebarComponent implements OnInit, OnDestroy, OnChanges {
	@Input() hasCompany: boolean | null = true;
	@Input() isExpanded: boolean = false;
	@Output() toggleSidebar = new EventEmitter<void>();

	navigationItems: NavigationItem[] = [];
	navigationState: NavigationState = {
		currentRoute: '',
		expandedSections: {},
		activeSection: undefined
	};
	companyId: number | null = null;

	private userService = inject(UserService);
	private branchService = inject(BranchService);

	private navigationSubscription: Subscription = new Subscription();

	// Datos dinámicos (ejemplo para sucursales)
	sucursales: BranchByCompanyMod[] = [];

	constructor(private navigationService: AdminNavService) { }

	ngOnInit(): void {
		// Suscribirse a cambios de estado de navegación
		this.navigationSubscription = this.navigationService.navigationState$
			.subscribe(state => {
				this.navigationState = state;
			});

		// Suscribirse a la recarga de sucursales
		this.navigationService.refreshBranches$
			.subscribe(() => {
				if (this.companyId) {
					this.cargarSucursales();
				}
			});

		this.userService.hasCompany().subscribe({
			next: (res) => {
				if (res.hasCompany && res.companyId) {
					this.companyId = res.companyId;
					this.cargarSucursales();
				}
			},
			error: (err) => console.error('Error obteniendo empresa del usuario:', err)
		});
	}


	ngOnChanges(changes: SimpleChanges): void {
		if (changes['hasCompany']) {
			this.updateNavigation();
		}
	}

	cargarSucursales(): void {
		this.branchService.getByIdCompany(this.companyId).subscribe({
			next: (data) => {
				this.sucursales = data;
				this.addDynamicSucursales();
			},
			error: (err) => {
				console.log('Error al cargar los datos:', err);
				const mensajeCompleto = err?.error?.message || 'Ocurrio un error inesperado.';
				const mensaje = mensajeCompleto.split(':')[1]?.trim() || mensajeCompleto;
				Swal.fire({
					icon: 'error',
					title: 'Error',
					text: mensaje,
					confirmButtonText: 'Aceptar'
				});
			}
		});
	}


	private updateNavigation(): void {
		// Obtener configuración basada en el estado de la empresa
		this.navigationItems = this.navigationService.getNavigationConfig(this.hasCompany ?? true);

		// Solo agregar sucursales si tiene empresa
		if (this.hasCompany) {
			this.addDynamicSucursales();
		}
	}

	ngOnDestroy(): void {
		if (this.navigationSubscription) {
			this.navigationSubscription.unsubscribe();
		}
	}

	private addDynamicSucursales(): void {
		const dynamicSucursales: NavigationItem[] = this.sucursales.map(sucursal => ({
			id: `dynamic-sucursal-${sucursal.id}`,
			label: sucursal.name,
			icon: 'store',
			route: `/admin/branch/${sucursal.id}`
		}));

		this.navigationService.addDynamicItems('sucursales', dynamicSucursales);
		// Actualizar la navegación después de agregar items dinámicos
		this.navigationItems = this.navigationService.getNavigationConfig(this.hasCompany ?? true);
	}

	// Métodos del template
	isRouteActive(route: string): boolean {
		return this.navigationService.isRouteActive(route);
	}

	isSectionActive(sectionId: string): boolean {
		return this.navigationService.isSectionActive(sectionId);
	}

	isSectionExpanded(sectionId: string): boolean {
		return this.navigationService.isSectionExpanded(sectionId);
	}

	toggleSection(sectionId: string): void {
		this.navigationService.toggleSection(sectionId);
	}

	navigateTo(route: string): void {
		this.navigationService.navigateTo(route);

		// Solo cerrar el sidebar en dispositivos móviles
		if (window.innerWidth < 768) {
			this.onToggleSidebar();
		}
	}

	// Event listeners
	@HostListener('document:keydown.escape', ['$event'])
	onEscapeKey(event: KeyboardEvent) {
		if (this.isExpanded) {
			this.onToggleSidebar();
			event.preventDefault();
		}
	}

	@HostListener('document:click', ['$event'])
	onClickOutside(event: Event) {
		const target = event.target as HTMLElement;
		if (this.isExpanded && !target.closest('.sidebar') &&
			!target.closest('.sidebar-toggle') &&
			!target.closest('.sidebar-overlay')) {
			this.onToggleSidebar();
		}
	}

	@HostListener('window:resize', ['$event'])
	onResize(event: any) {
		if (window.innerWidth >= 768 && !this.isExpanded) {
			this.onToggleSidebar();
		}
	}

	onToggleSidebar(): void {
		this.toggleSidebar.emit();
	}
}
