import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { BehaviorSubject, Subject } from 'rxjs';
import { filter } from 'rxjs/operators';

export interface NavigationItem {
	id: string;
	label: string;
	icon: string;
	route?: string;
	children?: NavigationItem[];
	expandable?: boolean;
	external?: boolean;
}

export interface NavigationState {
	currentRoute: string;
	expandedSections: { [key: string]: boolean };
	activeSection?: string;
}

@Injectable({
	providedIn: 'root'
})
export class AreaManagerNavService {
	private closeSidebarSubject = new Subject<void>();
	public closeSidebar$ = this.closeSidebarSubject.asObservable();

	private refreshOperatingGroupsSubject = new Subject<void>();
	public refreshOperatingGroups$ = this.refreshOperatingGroupsSubject.asObservable();

	// Configuración de navegación
	private readonly navigationConfig: NavigationItem[] = [
		{
			id: 'dashboard',
			label: 'Dashboard',
			icon: 'dashboard',
			route: '/areaManager/dashboard',
			expandable: false
		},
		{
			id: 'inventory',
			label: 'Inventario',
			icon: 'warehouse',
			expandable: true,
			children: [
				{
					id: 'inventoryBase',
					label: 'Inventario Base',
					icon: 'inventory_2',
					route: '/areaManager/inventory-base'
				},
				{
					id: 'requestsInventary',
					label: 'RQS Inventario',
					icon: 'assignment_late',
					route: '/areaManager/requests-inventary'
				},
				{
					id: 'Reports',
					label: 'Reportes',
					icon: 'bar_chart',
					route: '/areaManager/reports'
				}
			]
		},
		{
			id: 'itemManagement',
			label: 'Gestión de Ítems',
			icon: 'category',
			route: '/areaManager/item-management',
			expandable: false,
		},
		{
			id: 'operatingGroups',
			label: 'Grupos Operativos',
			icon: 'group_work',
			expandable: true,
			children: [
				{
					id: 'newGroup',
					label: 'Nueva Grupo',
					icon: 'add',
					route: '/areaManager/register-group'
				}
				// Los elementos dinámicas se agregarán aquí
			]
		},
		{
			id: 'operating',
			label: 'Operativos',
			icon: 'engineering',
			route: '/areaManager/operating-list',
			expandable: false,
		},
		{
			id: 'fullInventories',
			label: 'Inventarios Realizados',
			icon: 'assignment_turned_in',
			route: '/areaManager/full-inventories',
			expandable: false,
		},
	];

	private navigationState = new BehaviorSubject<NavigationState>({
		currentRoute: '',
		expandedSections: {},
		activeSection: undefined
	});

	public navigationState$ = this.navigationState.asObservable();

	constructor(private router: Router) {
		this.initializeNavigation();
	}

	private initializeNavigation(): void {
		// Escuchar cambios de ruta
		this.router.events
			.pipe(filter(event => event instanceof NavigationEnd))
			.subscribe((event: NavigationEnd) => {
				this.updateNavigationState(event.urlAfterRedirects || event.url);
				this.closeSidebarSubject.next();
			});

		// Estado inicial
		const currentRoute = this.router.url === '/areaManager' ? '/areaManager/dashboard' : this.router.url;
		this.updateNavigationState(currentRoute);
	}

	// MÉTODO MODIFICADO: Ahora acepta el parámetro hasCompany
	getNavigationConfig(): NavigationItem[] {
		return [...this.navigationConfig];
	}

	private updateNavigationState(route: string): void {
		const currentState = this.navigationState.value;
		const activeSection = this.findActiveSectionByRoute(route);
		const expandedSections = this.calculateExpandedSections(route, currentState.expandedSections);

		this.navigationState.next({
			currentRoute: route,
			expandedSections,
			activeSection
		});
	}

	private findActiveSectionByRoute(route: string): string | undefined {
		for (const item of this.navigationConfig) {
			// Si es una sección expandible, verificar sus hijos
			if (item.expandable && item.children) {
				for (const child of item.children) {
					if (child.route && this.isRouteMatch(route, child.route)) {
						return item.id;
					}
				}
			}
			// Si es una sección simple
			else if (item.route && this.isRouteMatch(route, item.route)) {
				return item.id;
			}
		}
		return undefined;
	}

	private isRouteMatch(currentRoute: string, targetRoute: string): boolean {
		// Normalizar rutas
		const current = currentRoute.endsWith('/') ? currentRoute.slice(0, -1) : currentRoute;
		const target = targetRoute.endsWith('/') ? targetRoute.slice(0, -1) : targetRoute;

		return current === target || current.startsWith(target + '/');
	}

	private calculateExpandedSections(route: string, currentExpanded: { [key: string]: boolean }): { [key: string]: boolean } {
		const newExpanded = { ...currentExpanded };
		const activeSection = this.findActiveSectionByRoute(route);

		// Auto-expandir la sección activa si es expandible
		if (activeSection) {
			const section = this.findSectionById(activeSection);
			if (section?.expandable) {
				newExpanded[activeSection] = true;
			}
		}

		return newExpanded;
	}

	private findSectionById(id: string): NavigationItem | undefined {
		return this.navigationConfig.find(item => item.id === id);
	}

	// Métodos públicos para el componente
	getCurrentState(): NavigationState {
		return this.navigationState.value;
	}

	toggleSection(sectionId: string): void {
		const currentState = this.navigationState.value;
		const newExpanded = {
			...currentState.expandedSections,
			[sectionId]: !currentState.expandedSections[sectionId]
		};

		this.navigationState.next({
			...currentState,
			expandedSections: newExpanded
		});
	}

	isRouteActive(route: string): boolean {
		const currentRoute = this.navigationState.value.currentRoute;

		// Casos especiales para areaManager base y dashboard
		if (route === '/areaManager' && currentRoute === '/areaManager/dashboard') return true;
		if (route === '/areaManager/dashboard' && (currentRoute === '/areaManager/dashboard' || currentRoute === '/areaManager')) return true;

		return this.isRouteMatch(currentRoute, route);
	}

	isSectionActive(sectionId: string): boolean {
		return this.navigationState.value.activeSection === sectionId;
	}

	isSectionExpanded(sectionId: string): boolean {
		return !!this.navigationState.value.expandedSections[sectionId];
	}

	navigateTo(route: string): void {
		const targetRoute = route === '/areaManager' ? '/areaManager/dashboard' : route;
		this.router.navigate([targetRoute]);
	}

	// Método para agregar elementos dinámicos (como zonas)
	addDynamicItems(parentSectionId: string, items: NavigationItem[]): void {
		const parentIndex = this.navigationConfig.findIndex(item => item.id === parentSectionId);
		if (parentIndex !== -1 && this.navigationConfig[parentIndex].children) {
			// Filtrar elementos estáticos existentes y agregar los dinámicos
			const staticItems = this.navigationConfig[parentIndex].children!.filter(child =>
				!child.id.includes('dynamic-')
			);
			this.navigationConfig[parentIndex].children = [...staticItems, ...items];
		}
	}

	triggerRefreshOperatingGroups(): void {
		this.refreshOperatingGroupsSubject.next();
	}

	// Método para limpiar elementos dinámicos
	clearDynamicItems(parentSectionId: string): void {
		const parentIndex = this.navigationConfig.findIndex(item => item.id === parentSectionId);
		if (parentIndex !== -1 && this.navigationConfig[parentIndex].children) {
			this.navigationConfig[parentIndex].children = this.navigationConfig[parentIndex].children!.filter(child =>
				!child.id.includes('dynamic-')
			);
		}
	}
}
