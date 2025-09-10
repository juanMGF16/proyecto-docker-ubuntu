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
export class AdminNavService {
	private closeSidebarSubject = new Subject<void>();
	public closeSidebar$ = this.closeSidebarSubject.asObservable();

	// Configuración de navegación
	private readonly navigationConfig: NavigationItem[] = [
		{
			id: 'dashboard',
			label: 'Dashboard',
			icon: 'dashboard',
			route: '/admin/dashboard',
			expandable: false
		},
		{
			id: 'empresa',
			label: 'Empresa',
			icon: 'business',
			expandable: true,
			children: [
				{
					id: 'empresa-registro',
					label: 'Registrar Empresa',
					icon: 'app_registration',
					route: '/admin/empresa/registro'
				},
				{
					id: 'empresa-configuracion',
					label: 'Configuración',
					icon: 'settings',
					route: '/admin/empresa/configuracion'
				}
			]
		},
		{
			id: 'sucursales',
			label: 'Sucursales',
			icon: 'location_city',
			expandable: true,
			children: [
				{
					id: 'sucursales-nueva',
					label: 'Nueva Sucursal',
					icon: 'add',
					route: '/admin/sucursales/nueva'
				}
				// Las sucursales dinámicas se agregarán aquí
			]
		},
		{
			id: 'administradores',
			label: 'Administradores',
			icon: 'supervisor_account',
			expandable: true,
			children: [
				{
					id: 'administradores-nuevo',
					label: 'Nuevo Admin',
					icon: 'person_add',
					route: '/admin/administradores/nuevo'
				},
				{
					id: 'administradores-lista',
					label: 'Lista de Admins',
					icon: 'list',
					route: '/admin/administradores/lista'
				}
			]
		}
	];

	// Configuración mínima cuando no tiene empresa
	private readonly noCompanyNavigationConfig: NavigationItem[] = [
		{
			id: 'register-company',
			label: 'Registrar Empresa',
			icon: 'business',
			route: '/admin/register-company',
			expandable: false
		}
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
		const currentRoute = this.router.url === '/admin' ? '/admin/dashboard' : this.router.url;
		this.updateNavigationState(currentRoute);
	}

	// MÉTODO MODIFICADO: Ahora acepta el parámetro hasCompany
	getNavigationConfig(hasCompany: boolean = true): NavigationItem[] {
		if (!hasCompany) {
			return [...this.noCompanyNavigationConfig];
		}
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

		// Casos especiales para admin base y dashboard
		if (route === '/admin' && currentRoute === '/admin/dashboard') return true;
		if (route === '/admin/dashboard' && (currentRoute === '/admin/dashboard' || currentRoute === '/admin')) return true;

		return this.isRouteMatch(currentRoute, route);
	}

	isSectionActive(sectionId: string): boolean {
		return this.navigationState.value.activeSection === sectionId;
	}

	isSectionExpanded(sectionId: string): boolean {
		return !!this.navigationState.value.expandedSections[sectionId];
	}

	navigateTo(route: string): void {
		const targetRoute = route === '/admin' ? '/admin/dashboard' : route;
		this.router.navigate([targetRoute]);
	}

	// Método para agregar elementos dinámicos (como sucursales)
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
