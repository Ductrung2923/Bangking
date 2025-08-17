// confirm-staff.services.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface StaffDTO {
    userId: number;
    staffId: number;
    employeeCode: string;
    position: string;
    gender: boolean;
    dateOfBirth: string;
    hireDate: string;
    department: string;
    fullName?: string;
    email?: string;
    isActive?: boolean;
    photoUrl?: string;
    notes?: string;
    createdAt?: string;
}

@Injectable({
    providedIn: 'root'
})
export class ConfirmStaffService {
    private baseUrl = 'https://localhost:7253/api/ConfirmStaff';

    constructor(private http: HttpClient) { }


    getAllStaff(): Observable<StaffDTO[]> {
        return this.http.get<StaffDTO[]>(this.baseUrl);
    }


    updateStaffStatus(userId: number, isActive: boolean): Observable<any> {
        return this.http.put(`${this.baseUrl}/${userId}`, isActive, {
            headers: { 'Content-Type': 'application/json' }
        });
    }


}
