export interface DashboardModel {
  totalBranches: number;
  totalZones: number;
  totalItems: number;
  usersByRole: Record<string, number>;
  itemsByCategory: Record<string, number>;
  itemsByState: Record<string, number>;
}
