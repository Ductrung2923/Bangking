import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AccountDTO {
    accountId: number;
    accountNumber: string;
    balance: number;
    fullName: string;
    email: string;
    phone: string;
    identityNumber: string;
    customerID?: number;
}

export interface DepositDTO {
    accountNumber: string;
    amount: number;
    description: string;
    staffId: number;
}

@Injectable({
    providedIn: 'root'
})
export class DepositService {
    private apiUrl = 'https://localhost:7253/api/Transaction';

    constructor(private http: HttpClient) { }

    checkAccount(accountNumber: string): Observable<AccountDTO> {
        return this.http.get<AccountDTO>(`${this.apiUrl}/check-account/${accountNumber}`);
    }

    deposit(payload: DepositDTO): Observable<any> {
        return this.http.post(`${this.apiUrl}/deposit`, payload);
    }
}
