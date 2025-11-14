export interface StockLevel {
  id: string;
  deviceId: string;
  deviceName?: string;
  deviceSerialNumber?: string;
  categoryName?: string;
  brandName?: string;
  warehouseId: string;
  warehouseName?: string;
  quantityOnHand: number;
  quantityReserved: number;
  quantityAvailable: number;
  reorderLevel: number;
  maxStockLevel: number;
  minStockLevel: number;
  unitCost: number;
  totalValue: number;
  location?: string;
  binNumber?: string;
  shelfNumber?: string;
  lastCountDate: string;
  nextCountDate: string;
  countFrequency: number;
  isActive: boolean;
  needsReorder: boolean;
  isOverstocked: boolean;
  isBelowMinimum: boolean;
  reorderQuantity: number;
  createdAt: string;
  updatedAt: string;
}

export interface StockLevelForCreation {
  deviceId: string;
  warehouseId: string;
  quantityOnHand: number;
  quantityReserved: number;
  reorderLevel: number;
  maxStockLevel: number;
  minStockLevel: number;
  unitCost: number;
  location?: string;
  binNumber?: string;
  shelfNumber?: string;
  countFrequency: number;
}

export interface StockLevelForUpdate {
  quantityOnHand: number;
  quantityReserved: number;
  reorderLevel: number;
  maxStockLevel: number;
  minStockLevel: number;
  unitCost: number;
  location?: string;
  binNumber?: string;
  shelfNumber?: string;
  countFrequency: number;
  isActive: boolean;
}

export interface StockAdjustment {
  stockLevelId: string;
  adjustmentQuantity: number;
  reason?: string;
  adjustmentType: AdjustmentType;
}

export enum AdjustmentType {
  Increase = 1,
  Decrease = 2,
  Set = 3
}