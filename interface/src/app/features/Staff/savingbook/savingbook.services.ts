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

export interface SavingDTO {
    customerId: number;
    accountNumber: string;
    amount: number;
    interestRate: number;
    termMonths: number;
    receiveInterestMethod: string;
    staffId: number;
}

@Injectable({
    providedIn: 'root'
})
export class SavingsbookService {
    private apiUrl = 'https://localhost:7253/api/Transaction';

    constructor(private http: HttpClient) { }

    checkAccount(accountNumber: string): Observable<AccountDTO> {
        return this.http.get<AccountDTO>(`${this.apiUrl}/check-account/${accountNumber}`);
    }

    createSaving(dto: SavingDTO): Observable<any> {
        return this.http.post(`${this.apiUrl}/saving`, dto);
    }
}

