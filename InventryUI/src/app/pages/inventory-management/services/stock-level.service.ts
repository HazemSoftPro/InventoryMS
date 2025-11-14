import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { 
  StockLevel, 
  StockLevelForCreation, 
  StockLevelForUpdate,
  StockAdjustment,
  AdjustmentType
} from '../models/stock-level.model';

@Injectable({
  providedIn: 'root'
})
export class StockLevelService {
  private baseUrl = environment.apiUrl + '/api/stock-levels';

  constructor(private http: HttpClient) { }

  getAllStockLevels(): Observable<StockLevel[]> {
    return this.http.get<StockLevel[]>(this.baseUrl);
  }

  getStockLevelById(id: string): Observable<StockLevel> {
    return this.http.get<StockLevel>(`${this.baseUrl}/${id}`);
  }

  getStockLevelsByDevice(deviceId: string): Observable<StockLevel[]> {
    return this.http.get<StockLevel[]>(`${this.baseUrl}/device/${deviceId}`);
  }

  getStockLevelsByWarehouse(warehouseId: string): Observable<StockLevel[]> {
    return this.http.get<StockLevel[]>(`${this.baseUrl}/warehouse/${warehouseId}`);
  }

  getLowStockItems(): Observable<StockLevel[]> {
    return this.http.get<StockLevel[]>(`${this.baseUrl}/low-stock`);
  }

  getOverstockItems(): Observable<StockLevel[]> {
    return this.http.get<StockLevel[]>(`${this.baseUrl}/overstock`);
  }

  createStockLevel(stockLevel: StockLevelForCreation): Observable<StockLevel> {
    return this.http.post<StockLevel>(this.baseUrl, stockLevel);
  }

  updateStockLevel(id: string, stockLevel: StockLevelForUpdate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, stockLevel);
  }

  adjustStockLevel(adjustment: StockAdjustment): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/adjust`, adjustment);
  }

  deleteStockLevel(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getAdjustmentTypes(): { value: AdjustmentType; label: string }[] {
    return [
      { value: AdjustmentType.Increase, label: 'Increase' },
      { value: AdjustmentType.Decrease, label: 'Decrease' },
      { value: AdjustmentType.Set, label: 'Set to Value' }
    ];
  }

  getStockLevelStatus(stockLevel: StockLevel): string {
    if (stockLevel.isBelowMinimum) return 'Below Minimum';
    if (stockLevel.needsReorder) return 'Needs Reorder';
    if (stockLevel.isOverstocked) return 'Overstocked';
    return 'Normal';
  }

  getStockLevelStatusClass(stockLevel: StockLevel): string {
    if (stockLevel.isBelowMinimum) return 'status-critical';
    if (stockLevel.needsReorder) return 'status-warning';
    if (stockLevel.isOverstocked) return 'status-info';
    return 'status-success';
  }
}