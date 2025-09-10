import { CommonModule } from '@angular/common';
import { Component, EventEmitter, HostListener, Input, Output, OnInit, OnDestroy, OnChanges, SimpleChanges } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Subscription } from 'rxjs';
import { AdminNavService, NavigationItem, NavigationState } from '../../../../Core/Service/Navigation/admin-nav.service';

@Component({
	selector: 'app-admin-sidebar',
	imports: [CommonModule, MatIconModule, MatButtonModule],
	standalone: true,
	templateUrl: './admin-sidebar.component.html',
	styleUrl: './admin-sidebar.component.css'
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

	private navigationSubscription: Subscription = new Subscription();

	// Datos dinámicos (ejemplo para sucursales)
	sucursales = [
		{ id: 1, nombre: 'Sucursal A' },
		{ id: 2, nombre: 'Sucursal B' },
		{ id: 3, nombre: 'Sucursal C' }
	];

	constructor(private navigationService: AdminNavService) { }

	ngOnInit(): void {
		// Obtener configuración de navegación basada en hasCompany
		this.updateNavigation();

		// Suscribirse a cambios de estado de navegación
		this.navigationSubscription = this.navigationService.navigationState$
			.subscribe(state => {
				this.navigationState = state;
			});
	}

	ngOnChanges(changes: SimpleChanges): void {
  if (changes['hasCompany']) {
    this.updateNavigation();
  }
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
			label: sucursal.nombre,
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
