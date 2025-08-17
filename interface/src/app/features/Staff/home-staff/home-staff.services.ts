import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CustomerDTO {
    fullName: string;
    email: string;
    photoUrl: string;
}

export interface TransactionHistoryDTO {
    transactionType: string;
    fromAccountNumber: string;
    toAccountNumber: string;
    amount: number;
    createdAt: string;
    status: string;
}

@Injectable({
    providedIn: 'root'
})
export class HomeStaffService {
    private newCustomerUrl = 'https://localhost:7253/api/Homeadd/newcustomer';
    private transactionHistoryUrl = 'https://localhost:7253/api/Transaction/history';

    constructor(private http: HttpClient) { }

    private getAuthHeaders(): HttpHeaders {
        const token = sessionStorage.getItem('token');
        return new HttpHeaders({
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        });
    }

    getRecentCustomers(count: number = 5): Observable<CustomerDTO[]> {
        return this.http.get<CustomerDTO[]>(this.newCustomerUrl, {
            headers: this.getAuthHeaders()
        });
    }

    getTransactionHistory(): Observable<TransactionHistoryDTO[]> {
        return this.http.get<TransactionHistoryDTO[]>(this.transactionHistoryUrl, {
            headers: this.getAuthHeaders()
        });
    }
}
