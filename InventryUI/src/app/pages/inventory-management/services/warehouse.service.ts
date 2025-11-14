import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { 
  Warehouse, 
  WarehouseForCreation, 
  WarehouseForUpdate,
  WarehouseStatistics
} from '../models/warehouse.model';

@Injectable({
  providedIn: 'root'
})
export class WarehouseService {
  private baseUrl = environment.apiUrl + '/api/warehouses';

  constructor(private http: HttpClient) { }

  getAllWarehouses(): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(this.baseUrl);
  }

  getWarehouseById(id: string): Observable<Warehouse> {
    return this.http.get<Warehouse>(`${this.baseUrl}/${id}`);
  }

  getWarehouseStatistics(id: string): Observable<WarehouseStatistics> {
    return this.http.get<WarehouseStatistics>(`${this.baseUrl}/${id}/statistics`);
  }

  getActiveWarehouses(): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(`${this.baseUrl}/active`);
  }

  getWarehousesWithAvailableCapacity(): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(`${this.baseUrl}/available-capacity`);
  }

  createWarehouse(warehouse: WarehouseForCreation): Observable<Warehouse> {
    return this.http.post<Warehouse>(this.baseUrl, warehouse);
  }

  updateWarehouse(id: string, warehouse: WarehouseForUpdate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, warehouse);
  }

  deleteWarehouse(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  updateWarehouseUtilization(id: string, utilization: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/utilization`, utilization);
  }

  getUtilizationClass(percentage: number): string {
    if (percentage >= 90) return 'utilization-critical';
    if (percentage >= 75) return 'utilization-warning';
    if (percentage >= 50) return 'utilization-good';
    return 'utilization-low';
  }

  getUtilizationStatus(percentage: number): string {
    if (percentage >= 90) return 'Critical';
    if (percentage >= 75) return 'High';
    if (percentage >= 50) return 'Good';
    return 'Low';
  }
}