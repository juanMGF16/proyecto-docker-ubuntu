import { Component, Input, Output, EventEmitter, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { Router } from '@angular/router';
import { LoaderComponent } from '../../app-loader/app-loader.component';
import { StaffFilterPipe } from '../../../../Core/Pipes/staff-filter.pipe';


export interface TableColumn {
  key: string;
  label: string;
  type?: 'text' | 'icon' | 'action' | 'custom';
  icon?: string;
  formatter?: (value: any, row: any) => string;
}

export interface TableConfig {
  title: string;
  subtitle: string;
  emptyState: {
    icon: string;
    title: string;
    description: string;
    buttonText?: string;
    buttonIcon?: string;
    buttonAction?: () => void;
  };
  columns: TableColumn[];
  modalSections: ModalSection[];
}

export interface ModalSection {
  title: string;
  icon: string;
  fields: ModalField[];
}

export interface ModalField {
  key: string;
  label: string;
  formatter?: (value: any) => string;
}

@Component({
  selector: 'app-show-staff',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    LoaderComponent,
		StaffFilterPipe
  ],
  templateUrl: './show-staff.component.html',
  styleUrls: ['../../Styles/modal-shared.css','./show-staff.component.css']
})
export class ShowStaffComponent {
  private router = inject(Router);

  @Input() data: any[] = [];
  @Input() loading = false;
  @Input() error: string | null = null;
  @Input() config!: TableConfig;
  @Input() searchPlaceholder = 'Buscar...';

  @Output() rowClick = new EventEmitter<any>();
  @Output() buttonAction = new EventEmitter<void>();

  selectedItem = signal<any>(null);
  isModalOpen = signal(false);
  searchText = '';

  get displayedColumns(): string[] {
    return this.config.columns.map(col => col.key).concat('actions');
  }

  get hasData(): boolean {
    return this.data.length > 0;
  }

  onViewDetails(item: any): void {
    this.selectedItem.set(item);
    this.isModalOpen.set(true);
    this.rowClick.emit(item);
  }

  closeModal(): void {
    this.isModalOpen.set(false);
    this.selectedItem.set(null);
  }

  onButtonAction(): void {
    this.buttonAction.emit();
  }

  getCellValue(item: any, column: TableColumn): string {
    const value = item[column.key];
    if (column.formatter) {
      return column.formatter(value, item);
    }
    return value || '';
  }
}
