import { Routes } from '@angular/router';
import { AppDashboardComponent } from './dashboard/dashboard.component';
import { InventoryManagementComponent } from './inventory-management/inventory-management.component';

export const PagesRoutes: Routes = [
  {
    path: '',
    component: AppDashboardComponent,
    data: {
      title: 'Starter Page',
    },
  },
  {
    path: 'inventory-management',
    component: InventoryManagementComponent,
    data: {
      title: 'Inventory Management',
    },
  },
];
