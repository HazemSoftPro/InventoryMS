import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { WarehouseService } from './services/warehouse.service';
import { StockLevelService } from './services/stock-level.service';
import { InventoryTransactionService } from './services/inventory-transaction.service';
import { PurchaseOrderService } from './services/purchase-order.service';
import { Warehouse, WarehouseStatistics } from './models/warehouse.model';
import { StockLevel } from './models/stock-level.model';
import { InventoryTransaction } from './models/inventory-transaction.model';
import { PurchaseOrder } from './models/purchase-order.model';

@Component({
  selector: 'app-inventory-management',
  templateUrl: './inventory-management.component.html',
  styleUrls: ['./inventory-management.component.scss']
})
export class InventoryManagementComponent implements OnInit {
  warehouses$!: Observable<Warehouse[]>;
  lowStockItems$!: Observable<StockLevel[]>;
  recentTransactions$!: Observable<InventoryTransaction[]>;
  pendingOrders$!: Observable<PurchaseOrder[]>;

  // Dashboard statistics
  totalWarehouses = 0;
  activeWarehouses = 0;
  totalInventoryValue = 0;
  lowStockCount = 0;
  overstockCount = 0;
  pendingApprovals = 0;

  // Loading state
  loading = false;
  error: string | null = null;

  // Selected warehouse for detailed view
  selectedWarehouse: Warehouse | null = null;
  warehouseStatistics: WarehouseStatistics | null = null;

  constructor(
    private router: Router,
    private warehouseService: WarehouseService,
    private stockLevelService: StockLevelService,
    private transactionService: InventoryTransactionService,
    private purchaseOrderService: PurchaseOrderService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.loading = true;
    this.error = null;

    // Load all dashboard data
    this.warehouses$ = this.warehouseService.getAllWarehouses();
    this.lowStockItems$ = this.stockLevelService.getLowStockItems();
    this.recentTransactions$ = this.transactionService.getAllTransactions();
    this.pendingOrders$ = this.purchaseOrderService.getPendingApprovalOrders();

    // Load statistics
    this.loadStatistics();

    this.loading = false;
  }

  private async loadStatistics(): Promise<void> {
    try {
      // Get warehouses and calculate statistics
      const warehouses = await this.warehouses$.toPromise();
      if (warehouses) {
        this.totalWarehouses = warehouses.length;
        this.activeWarehouses = warehouses.filter(w => w.isActive).length;
        this.totalInventoryValue = warehouses.reduce((sum, w) => sum + w.totalInventoryValue, 0);
      }

      // Get low stock items count
      const lowStockItems = await this.lowStockItems$.toPromise();
      if (lowStockItems) {
        this.lowStockCount = lowStockItems.length;
      }

      // Get overstock items count
      const overstockItems = await this.stockLevelService.getOverstockItems().toPromise();
      if (overstockItems) {
        this.overstockCount = overstockItems.length;
      }

      // Get pending approvals count
      const pendingOrders = await this.pendingOrders$.toPromise();
      if (pendingOrders) {
        this.pendingApprovals = pendingOrders.length;
      }
    } catch (error) {
      console.error('Error loading statistics:', error);
      this.error = 'Failed to load dashboard statistics';
    }
  }

  viewWarehouseDetails(warehouse: Warehouse): void {
    this.selectedWarehouse = warehouse;
    this.loadWarehouseStatistics(warehouse.id);
  }

  private loadWarehouseStatistics(warehouseId: string): void {
    this.warehouseService.getWarehouseStatistics(warehouseId).subscribe({
      next: (stats) => {
        this.warehouseStatistics = stats;
      },
      error: (err) => {
        console.error('Error loading warehouse statistics:', err);
        this.error = 'Failed to load warehouse statistics';
      }
    });
  }

  navigateToTransactions(): void {
    this.router.navigate(['/inventory-management/transactions']);
  }

  navigateToStockLevels(): void {
    this.router.navigate(['/inventory-management/stock-levels']);
  }

  navigateToWarehouses(): void {
    this.router.navigate(['/inventory-management/warehouses']);
  }

  navigateToPurchaseOrders(): void {
    this.router.navigate(['/inventory-management/purchase-orders']);
  }

  refreshData(): void {
    this.loadDashboardData();
  }

  clearSelection(): void {
    this.selectedWarehouse = null;
    this.warehouseStatistics = null;
  }

  getUtilizationClass(percentage: number): string {
    return this.warehouseService.getUtilizationClass(percentage);
  }

  getStockLevelStatusClass(stockLevel: StockLevel): string {
    return this.stockLevelService.getStockLevelStatusClass(stockLevel);
  }

  getStatusClass(status: any): string {
    return this.purchaseOrderService.getStatusClass(status);
  }
}