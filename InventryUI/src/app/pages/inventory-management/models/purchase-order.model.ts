export interface PurchaseOrder {
  id: string;
  orderNumber: string;
  supplierId: string;
  supplierName?: string;
  officeId?: string;
  officeName?: string;
  warehouseId?: string;
  warehouseName?: string;
  orderDate: string;
  expectedDeliveryDate?: string;
  actualDeliveryDate?: string;
  subtotal: number;
  taxAmount: number;
  shippingAmount: number;
  totalAmount: number;
  currency?: string;
  status: OrderStatus;
  statusDisplay: string;
  notes?: string;
  terms?: string;
  shippingAddress?: string;
  billingAddress?: string;
  contactPerson?: string;
  contactEmail?: string;
  contactPhone?: string;
  createdByName?: string;
  approvedByName?: string;
  approvedDate?: string;
  createdAt: string;
  updatedAt: string;
  items?: PurchaseOrderItem[];
}

export interface PurchaseOrderForCreation {
  supplierId: string;
  officeId?: string;
  warehouseId?: string;
  expectedDeliveryDate?: string;
  notes?: string;
  terms?: string;
  shippingAddress?: string;
  billingAddress?: string;
  contactPerson?: string;
  contactEmail?: string;
  contactPhone?: string;
  items?: PurchaseOrderItemForCreation[];
}

export interface PurchaseOrderForUpdate {
  expectedDeliveryDate?: string;
  notes?: string;
  terms?: string;
  shippingAddress?: string;
  billingAddress?: string;
  contactPerson?: string;
  contactEmail?: string;
  contactPhone?: string;
  status: OrderStatus;
}

export interface PurchaseOrderItem {
  id: string;
  purchaseOrderId: string;
  deviceId: string;
  deviceName?: string;
  deviceSerialNumber?: string;
  quantity: number;
  unitPrice: number;
  discountPercentage: number;
  discountAmount: number;
  taxPercentage: number;
  taxAmount: number;
  totalPrice: number;
  description?: string;
  partNumber?: string;
  manufacturer?: string;
  expectedDeliveryDate?: string;
  quantityReceived: number;
  isFullyReceived: boolean;
  isPartiallyReceived: boolean;
}

export interface PurchaseOrderItemForCreation {
  deviceId: string;
  quantity: number;
  unitPrice: number;
  discountPercentage: number;
  taxPercentage: number;
  description?: string;
  partNumber?: string;
  manufacturer?: string;
  expectedDeliveryDate?: string;
}

export interface PurchaseOrderItemReceive {
  itemId: string;
  quantityReceived: number;
}

export enum OrderStatus {
  Draft = 1,
  Sent = 2,
  Approved = 3,
  Rejected = 4,
  PartiallyReceived = 5,
  Received = 6,
  Cancelled = 7,
  Closed = 8
}