import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { Chart, ChartConfiguration, registerables } from 'chart.js';

// Registramos todos los elementos de Chart.js
Chart.register(...registerables);

@Component({
	selector: 'app-category-chart',
	standalone: true,
	imports: [CommonModule],
	templateUrl: './category-chart.component.html',
	styleUrl: './category-chart.component.css'
})
export class CategoryChartComponent implements OnChanges, OnDestroy {
	@Input() data!: { [category: string]: number };

	private chart: Chart | null = null;

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
		const ctx = document.getElementById('categoryChart') as HTMLCanvasElement;

		if (!ctx) return;

		const categories = Object.keys(this.data);
		const counts = Object.values(this.data);

		const config: ChartConfiguration = {
			type: 'bar',
			data: {
				labels: categories,
				datasets: [{
					label: 'Ítems por Categoría',
					data: counts,
					backgroundColor: this.generateGradientColors(categories.length),
					borderColor: 'rgba(255, 255, 255, 0.1)',
					borderWidth: 1,
					borderRadius: 6,
					hoverBackgroundColor: this.generateHoverColors(categories.length)
				}]
			},
			options: {
				responsive: true,
				maintainAspectRatio: false,
				plugins: {
					legend: {
						display: false,
					},
					title: {
						display: true,
						text: 'Distribución por Categoría',
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
						boxPadding: 6
					}
				},
				scales: {
					y: {
						beginAtZero: true,
						grid: {
							color: 'rgba(255, 255, 255, 0.1)'
						},
						ticks: {
							color: 'rgba(255, 255, 255, 0.7)'
						}
					},
					x: {
						grid: {
							display: false
						},
						ticks: {
							color: 'rgba(255, 255, 255, 0.7)',
							font: {
								size: 12
							}
						}
					}
				}
			}
		};

		this.chart = new Chart(ctx, config);
	}

	private generateGradientColors(count: number): string[] {
		const baseColors = [
			'rgba(59, 130, 246, 0.8)',
			'rgba(139, 92, 246, 0.8)',
			'rgba(16, 185, 129, 0.8)',
			'rgba(245, 158, 11, 0.8)',
			'rgba(239, 68, 68, 0.8)',
			'rgba(236, 72, 153, 0.8)',
			'rgba(14, 165, 233, 0.8)'
		];

		return Array.from({ length: count }, (_, i) => baseColors[i % baseColors.length]);
	}

	private generateHoverColors(count: number): string[] {
		const baseColors = [
			'rgba(59, 130, 246, 1)',
			'rgba(139, 92, 246, 1)',
			'rgba(16, 185, 129, 1)',
			'rgba(245, 158, 11, 1)',
			'rgba(239, 68, 68, 1)',
			'rgba(236, 72, 153, 1)',
			'rgba(14, 165, 233, 1)'
		];

		return Array.from({ length: count }, (_, i) => baseColors[i % baseColors.length]);
	}
}
