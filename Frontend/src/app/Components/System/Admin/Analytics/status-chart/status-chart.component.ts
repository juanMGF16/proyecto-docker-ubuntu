import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges, OnDestroy } from '@angular/core';
import { Chart, ChartConfiguration, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
  selector: 'app-status-chart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './status-chart.component.html',
  styleUrl: './status-chart.component.css'
})
export class StatusChartComponent implements OnChanges, OnDestroy {
  @Input() data!: { [status: string]: number };

  private chart: Chart<'doughnut'> | null = null;

  ngOnChanges(changes: SimpleChanges) {
    if (changes['data'] && this.data) {
      this.updateChart();
    }
  }

  ngOnDestroy() {
    if (this.chart) {
      this.chart.destroy();
    }
  }

  private updateChart() {
    // Destruir el gráfico existente si hay uno
    if (this.chart) {
      this.chart.destroy();
    }

    this.createChart();
  }

  private createChart() {
    const ctx = document.getElementById('statusChart') as HTMLCanvasElement;
    if (!ctx) return;

    // Verificar que hay datos disponibles
    if (!this.data || Object.keys(this.data).length === 0) {
      return;
    }

    const statuses = Object.keys(this.data);
    const counts = Object.values(this.data);

    const config: ChartConfiguration<'doughnut'> = {
      type: 'doughnut',
      data: {
        labels: statuses,
        datasets: [
          {
            data: counts,
            backgroundColor: this.generateStatusColors(statuses),
            borderColor: 'rgba(255, 255, 255, 0.1)',
            borderWidth: 2,
            hoverBackgroundColor: this.generateStatusHoverColors(statuses),
            hoverOffset: 8
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        cutout: '70%',
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              color: 'rgba(255, 255, 255, 0.8)',
              font: {
                size: 12,
                weight: 500
              },
              padding: 20,
              usePointStyle: true,
              pointStyle: 'circle'
            }
          },
          title: {
            display: true,
            text: 'Distribución por Estado',
            color: 'rgba(255, 255, 255, 0.8)',
            font: {
              size: 16,
              weight: 600
            }
          },
          tooltip: {
            backgroundColor: 'rgba(30, 58, 138, 0.9)',
            titleColor: '#fff',
            bodyColor: '#fff',
            borderColor: 'rgba(255, 255, 255, 0.1)',
            borderWidth: 1,
            padding: 12,
            boxPadding: 6,
            callbacks: {
              label: (context) => {
                const data = context.dataset.data as number[];
                const total = data.reduce((a, b) => a + b, 0);
                const percentage =
                  total > 0
                    ? Math.round(((context.parsed as number) / total) * 100)
                    : 0;
                return `${context.label}: ${context.parsed} (${percentage}%)`;
              }
            }
          }
        }
      }
    };

    this.chart = new Chart<'doughnut'>(ctx, config);
  }

  private generateStatusColors(statuses: string[]): string[] {
    const colorMap: { [key: string]: string } = {
      'En orden': 'rgba(16, 185, 129, 0.8)', // Verde
      'Reparación': 'rgba(245, 158, 11, 0.8)', // Amarillo
      'Dañado': 'rgba(239, 68, 68, 0.8)', // Rojo
      'Perdido': 'rgba(156, 163, 175, 0.8)' // Gris
    };

    return statuses.map(
      (status) => colorMap[status] || 'rgba(59, 130, 246, 0.8)'
    );
  }

  private generateStatusHoverColors(statuses: string[]): string[] {
    const colorMap: { [key: string]: string } = {
      'En orden': 'rgba(16, 185, 129, 1)',
      'Reparación': 'rgba(245, 158, 11, 1)',
      'Dañado': 'rgba(239, 68, 68, 1)',
      'Perdido': 'rgba(156, 163, 175, 1)'
    };

    return statuses.map(
      (status) => colorMap[status] || 'rgba(59, 130, 246, 1)'
    );
  }
}
