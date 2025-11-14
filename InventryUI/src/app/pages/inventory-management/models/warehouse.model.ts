export interface Warehouse {
  id: string;
  name: string;
  code?: string;
  address?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  phone?: string;
  email?: string;
  managerName?: string;
  managerContact?: string;
  capacity: number;
  currentUtilization: number;
  utilizationPercentage: number;
  isActive: boolean;
  isTemperatureControlled: boolean;
  isSecureStorage: boolean;
  operatingHours?: string;
  specialInstructions?: string;
  createdAt: string;
  updatedAt: string;
  createdByName?: string;
  updatedByName?: string;
  deviceCount: number;
  stockLevelCount: number;
  totalInventoryValue: number;
}

export interface WarehouseForCreation {
  name: string;
  code?: string;
  address?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  phone?: string;
  email?: string;
  managerName?: string;
  managerContact?: string;
  capacity: number;
  isTemperatureControlled: boolean;
  isSecureStorage: boolean;
  operatingHours?: string;
  specialInstructions?: string;
}

export interface WarehouseForUpdate {
  name: string;
  code?: string;
  address?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  phone?: string;
  email?: string;
  managerName?: string;
  managerContact?: string;
  capacity: number;
  isActive: boolean;
  isTemperatureControlled: boolean;
  isSecureStorage: boolean;
  operatingHours?: string;
  specialInstructions?: string;
}

export interface WarehouseStatistics {
  warehouseId: string;
  warehouseName: string;
  totalDevices: number;
  availableDevices: number;
  faultyDevices: number;
  assignedDevices: number;
  maintenanceDevices: number;
  totalValue: number;
  utilizationPercentage: number;
  lowStockItems: number;
  overstockItems: number;
  pendingTransactions: number;
  lastUpdated: string;
}