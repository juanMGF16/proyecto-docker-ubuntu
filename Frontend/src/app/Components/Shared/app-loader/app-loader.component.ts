import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
	selector: 'app-loader',
	standalone: true,
	imports: [CommonModule, MatProgressSpinnerModule, MatIconModule, MatButtonModule],
	template: `
    <!-- Estado cargando -->
    <div class="loader-container" [class.fullscreen]="fullScreen" *ngIf="loading">
      <mat-progress-spinner
        mode="indeterminate"
        [diameter]="diameter"
        [color]="color">
      </mat-progress-spinner>
      <p *ngIf="loadingMessage">{{ loadingMessage }}</p>
    </div>

    <!-- Estado error -->
    <div class="loader-container error" [class.fullscreen]="fullScreen" *ngIf="!loading && error">
      <mat-icon>error</mat-icon>
      <p>{{ error }}</p>
      <button mat-raised-button color="warn" *ngIf="showRetry" (click)="onRetry()">Reintentar</button>
    </div>
  `,
	styleUrls: ['./app-loader.component.css']
})
export class LoaderComponent {
	/** Estado de carga */
	@Input() loading: boolean = false;

	/** Mensaje de carga */
	@Input() loadingMessage: string = 'Cargando...';

	/** Mensaje de error */
	@Input() error: string | null = null;

	/** Mostrar overlay pantalla completa */
	@Input() fullScreen: boolean = false;

	/** Tamaño del spinner */
	@Input() diameter: number = 50;

	/** Color del spinner (primary, accent, warn) */
	@Input() color: 'primary' | 'accent' | 'warn' = 'primary';

	/** Mostrar botón de reintento */
	@Input() showRetry: boolean = false;

	/** Evento cuando se hace click en reintentar */
	@Output() retry = new EventEmitter<void>();

	onRetry() {
		this.retry.emit();
	}
}
