import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CustomerDTO {
    fullName: string;
    email: string;
    phone: string;
    address: string;
    identityNumber: string;
    dateOfBirth: string;
    gender: boolean;
    photoUrl: string;
    cardNumber: string;
    createdAt: string;
    accountNumber: string;
    balance: number;
    createdAtAc: string;

}

@Injectable({
    providedIn: 'root'
})
export class ListcusStaffService {
    private apiUrl = 'https://localhost:7253/api/ListCustomer';

    constructor(private http: HttpClient) { }

    private getAuthHeaders(): HttpHeaders {
        const token = sessionStorage.getItem('token');
        return new HttpHeaders({
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        });
    }

    getAllCustomers(): Observable<CustomerDTO[]> {
        return this.http.get<CustomerDTO[]>(this.apiUrl, { headers: this.getAuthHeaders() });
    }

    addCustomer(customer: any): Observable<any> {
        const token = sessionStorage.getItem('token');
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`
        });

        return this.http.post(this.apiUrl, customer, { headers });
    }


}
