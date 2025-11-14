export interface InventoryTransaction {
  id: string;
  deviceId: string;
  deviceName?: string;
  deviceSerialNumber?: string;
  fromUserId?: string;
  fromUserName?: string;
  toUserId?: string;
  toUserName?: string;
  fromOfficeId?: string;
  fromOfficeName?: string;
  toOfficeId?: string;
  toOfficeName?: string;
  transactionType: TransactionType;
  transactionTypeDisplay: string;
  description?: string;
  transactionDate: string;
  unitCost?: number;
  totalValue?: number;
  referenceNumber?: string;
  status: TransactionStatus;
  statusDisplay: string;
  notes?: string;
  approvedById?: string;
  approvedByName?: string;
  approvedDate?: string;
  createdAt: string;
  updatedAt: string;
}

export interface InventoryTransactionForCreation {
  deviceId: string;
  fromUserId?: string;
  toUserId?: string;
  fromOfficeId?: string;
  toOfficeId?: string;
  transactionType: TransactionType;
  description?: string;
  unitCost?: number;
  totalValue?: number;
  referenceNumber?: string;
  notes?: string;
}

export interface InventoryTransactionForUpdate {
  transactionType: TransactionType;
  description?: string;
  unitCost?: number;
  totalValue?: number;
  referenceNumber?: string;
  status: TransactionStatus;
  notes?: string;
}

export enum TransactionType {
  Purchase = 1,
  Transfer = 2,
  Assignment = 3,
  Return = 4,
  Disposal = 5,
  Repair = 6,
  Maintenance = 7,
  Audit = 8,
  Adjustment = 9
}

export enum TransactionStatus {
  Pending = 1,
  Approved = 2,
  Rejected = 3,
  Completed = 4,
  Cancelled = 5
}