import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { 
  PurchaseOrder, 
  PurchaseOrderForCreation, 
  PurchaseOrderForUpdate,
  PurchaseOrderItemReceive,
  OrderStatus
} from '../models/purchase-order.model';

@Injectable({
  providedIn: 'root'
})
export class PurchaseOrderService {
  private baseUrl = environment.apiUrl + '/api/purchase-orders';

  constructor(private http: HttpClient) { }

  getAllPurchaseOrders(): Observable<PurchaseOrder[]> {
    return this.http.get<PurchaseOrder[]>(this.baseUrl);
  }

  getPurchaseOrderById(id: string): Observable<PurchaseOrder> {
    return this.http.get<PurchaseOrder>(`${this.baseUrl}/${id}`);
  }

  getPurchaseOrdersBySupplier(supplierId: string): Observable<PurchaseOrder[]> {
    return this.http.get<PurchaseOrder[]>(`${this.baseUrl}/supplier/${supplierId}`);
  }

  getPurchaseOrdersByStatus(status: OrderStatus): Observable<PurchaseOrder[]> {
    return this.http.get<PurchaseOrder[]>(`${this.baseUrl}/status/${status}`);
  }

  getPendingApprovalOrders(): Observable<PurchaseOrder[]> {
    return this.http.get<PurchaseOrder[]>(`${this.baseUrl}/pending-approval`);
  }

  createPurchaseOrder(purchaseOrder: PurchaseOrderForCreation): Observable<PurchaseOrder> {
    return this.http.post<PurchaseOrder>(this.baseUrl, purchaseOrder);
  }

  updatePurchaseOrder(id: string, purchaseOrder: PurchaseOrderForUpdate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, purchaseOrder);
  }

  approvePurchaseOrder(id: string): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/approve`, {});
  }

  receivePurchaseOrder(id: string, items: PurchaseOrderItemReceive[]): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/receive`, items);
  }

  deletePurchaseOrder(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getOrderStatuses(): { value: OrderStatus; label: string }[] {
    return [
      { value: OrderStatus.Draft, label: 'Draft' },
      { value: OrderStatus.Sent, label: 'Sent' },
      { value: OrderStatus.Approved, label: 'Approved' },
      { value: OrderStatus.Rejected, label: 'Rejected' },
      { value: OrderStatus.PartiallyReceived, label: 'Partially Received' },
      { value: OrderStatus.Received, label: 'Received' },
      { value: OrderStatus.Cancelled, label: 'Cancelled' },
      { value: OrderStatus.Closed, label: 'Closed' }
    ];
  }

  getStatusClass(status: OrderStatus): string {
    switch (status) {
      case OrderStatus.Draft:
        return 'status-draft';
      case OrderStatus.Sent:
        return 'status-pending';
      case OrderStatus.Approved:
        return 'status-approved';
      case OrderStatus.Rejected:
        return 'status-rejected';
      case OrderStatus.PartiallyReceived:
        return 'status-partial';
      case OrderStatus.Received:
        return 'status-completed';
      case OrderStatus.Cancelled:
        return 'status-cancelled';
      case OrderStatus.Closed:
        return 'status-closed';
      default:
        return 'status-default';
    }
  }

  calculateOrderTotals(items: any[]): { subtotal: number; taxAmount: number; totalAmount: number } {
    let subtotal = 0;
    let taxAmount = 0;

    items.forEach(item => {
      const itemTotal = item.quantity * item.unitPrice;
      const discountAmount = itemTotal * (item.discountPercentage / 100);
      const discountedTotal = itemTotal - discountAmount;
      const itemTax = discountedTotal * (item.taxPercentage / 100);
      
      subtotal += discountedTotal;
      taxAmount += itemTax;
    });

    return {
      subtotal,
      taxAmount,
      totalAmount: subtotal + taxAmount
    };
  }
}