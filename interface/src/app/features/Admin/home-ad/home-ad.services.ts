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

export interface WeeklyTransactionStatDTO {
    day: string;
    count: number;
}

export interface TotalBalanceDTO {
    totalBalance: number;
}


@Injectable({
    providedIn: 'root'
})
export class HomeAdService {
    private baseUrl = 'https://localhost:7253/api/Homeadd';
    private transactionHistoryUrl = 'https://localhost:7253/api/Transaction/history';

    constructor(private http: HttpClient) { }

    private getAuthHeaders(): HttpHeaders {
        const token = sessionStorage.getItem('token');
        return new HttpHeaders({
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        });
    }

    // ✅ Danh sách khách hàng mới
    getRecentCustomers(): Observable<CustomerDTO[]> {
        return this.http.get<CustomerDTO[]>(`${this.baseUrl}/newcustomer`, {
            headers: this.getAuthHeaders()
        });
    }

    getTransactionHistory(): Observable<TransactionHistoryDTO[]> {
        return this.http.get<TransactionHistoryDTO[]>(this.transactionHistoryUrl, {
            headers: this.getAuthHeaders()
        });
    }

    // ✅ Tổng số nhân viên
    getTotalStaff(): Observable<number> {
        return this.http.get<number>(`${this.baseUrl}/total-staff`, {
            headers: this.getAuthHeaders()
        });
    }

    // ✅ Tổng số khách hàng
    getTotalCustomers(): Observable<number> {
        return this.http.get<number>(`${this.baseUrl}/total-customers`, {
            headers: this.getAuthHeaders()
        });
    }

    // ✅ Tổng số tài khoản ngân hàng
    getTotalAccounts(): Observable<number> {
        return this.http.get<number>(`${this.baseUrl}/total-accounts`, {
            headers: this.getAuthHeaders()
        });
    }

    // ✅ Tổng số ngân hàng ngoài
    getTotalExternalBanks(): Observable<number> {
        return this.http.get<number>(`${this.baseUrl}/total-externalbanks`, {
            headers: this.getAuthHeaders()
        });
    }

    // ✅ Số giao dịch trong tuần hiện tại
    getWeeklyTransactionStats(): Observable<WeeklyTransactionStatDTO[]> {
        return this.http.get<WeeklyTransactionStatDTO[]>(`${this.baseUrl}/weekly-transactions`, {
            headers: this.getAuthHeaders()
        });
    }

    getTotalBankBalance(): Observable<{ totalBalance: number }> {
        return this.http.get<{ totalBalance: number }>(`${this.baseUrl}/total-customer-balance`, {
            headers: this.getAuthHeaders()
        });
    }



}
