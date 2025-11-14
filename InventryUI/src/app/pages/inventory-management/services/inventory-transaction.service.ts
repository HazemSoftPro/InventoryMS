import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { 
  InventoryTransaction, 
  InventoryTransactionForCreation, 
  InventoryTransactionForUpdate,
  TransactionType,
  TransactionStatus 
} from '../models/inventory-transaction.model';

@Injectable({
  providedIn: 'root'
})
export class InventoryTransactionService {
  private baseUrl = environment.apiUrl + '/api/inventory-transactions';

  constructor(private http: HttpClient) { }

  getAllTransactions(): Observable<InventoryTransaction[]> {
    return this.http.get<InventoryTransaction[]>(this.baseUrl);
  }

  getTransactionById(id: string): Observable<InventoryTransaction> {
    return this.http.get<InventoryTransaction>(`${this.baseUrl}/${id}`);
  }

  getTransactionsByDevice(deviceId: string): Observable<InventoryTransaction[]> {
    return this.http.get<InventoryTransaction[]>(`${this.baseUrl}/device/${deviceId}`);
  }

  getTransactionsByUser(userId: string): Observable<InventoryTransaction[]> {
    return this.http.get<InventoryTransaction[]>(`${this.baseUrl}/user/${userId}`);
  }

  getTransactionsByType(transactionType: TransactionType): Observable<InventoryTransaction[]> {
    return this.http.get<InventoryTransaction[]>(`${this.baseUrl}/type/${transactionType}`);
  }

  getTransactionsByStatus(status: TransactionStatus): Observable<InventoryTransaction[]> {
    return this.http.get<InventoryTransaction[]>(`${this.baseUrl}/status/${status}`);
  }

  createTransaction(transaction: InventoryTransactionForCreation): Observable<InventoryTransaction> {
    return this.http.post<InventoryTransaction>(this.baseUrl, transaction);
  }

  updateTransaction(id: string, transaction: InventoryTransactionForUpdate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, transaction);
  }

  approveTransaction(id: string): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/approve`, {});
  }

  deleteTransaction(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getTransactionTypes(): { value: TransactionType; label: string }[] {
    return [
      { value: TransactionType.Purchase, label: 'Purchase' },
      { value: TransactionType.Transfer, label: 'Transfer' },
      { value: TransactionType.Assignment, label: 'Assignment' },
      { value: TransactionType.Return, label: 'Return' },
      { value: TransactionType.Disposal, label: 'Disposal' },
      { value: TransactionType.Repair, label: 'Repair' },
      { value: TransactionType.Maintenance, label: 'Maintenance' },
      { value: TransactionType.Audit, label: 'Audit' },
      { value: TransactionType.Adjustment, label: 'Adjustment' }
    ];
  }

  getTransactionStatuses(): { value: TransactionStatus; label: string }[] {
    return [
      { value: TransactionStatus.Pending, label: 'Pending' },
      { value: TransactionStatus.Approved, label: 'Approved' },
      { value: TransactionStatus.Rejected, label: 'Rejected' },
      { value: TransactionStatus.Completed, label: 'Completed' },
      { value: TransactionStatus.Cancelled, label: 'Cancelled' }
    ];
  }
}