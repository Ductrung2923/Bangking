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

@Injectable({
    providedIn: 'root'
})
export class TransactionService {
    private apiUrl = 'https://localhost:7253/api/Transaction';

    constructor(private http: HttpClient) { }

    checkAccount(accountNumber: string): Observable<any> {
        return this.http.get(`${this.apiUrl}/check-account/${accountNumber}`);
    }


    transferInternal(payload: {
        fromAccountNumber: string;
        toAccountNumber: string;
        amount: number;
        type: string;
        description: string;
        createdAt: string;
        status: string;
        staffId: number;
    }): Observable<any> {
        return this.http.post(`${this.apiUrl}/transfer-internal`, payload);
    }


}
