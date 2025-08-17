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

export interface ExternalBank {
    externalBankId: number;
    bankName: string;
}

export interface ExternalAccountDTO {
    accountNumber: string;
    externalBankID: number;
    accountHolderName: string;
    isFound: boolean;
}

export interface TransferExternalPayload {
    fromAccountNumber: string;
    toAccountNumber: string;
    externalBankID: number;
    amount: number;
    type: string;
    description: string;
    createdAt: string;
    status: string;
    staffId: number;
}


@Injectable({
    providedIn: 'root'
})
export class TransactionExternalService {
    private apiUrl = 'https://localhost:7253/api/Transaction';

    constructor(private http: HttpClient) { }

    checkAccount(accountNumber: string): Observable<any> {
        return this.http.get(`${this.apiUrl}/check-account/${accountNumber}`);
    }


    getAllExternalBanks(): Observable<ExternalBank[]> {
        return this.http.get<ExternalBank[]>(`${this.apiUrl}/external-banks`);
    }

    checkExternalAccount(bankId: number, accountNumber: string): Observable<ExternalAccountDTO> {
        const url = `${this.apiUrl}/check-external-account?bankId=${bankId}&accountNumber=${accountNumber}`;
        return this.http.get<ExternalAccountDTO>(url);
    }

    transferExternal(payload: TransferExternalPayload): Observable<any> {
        return this.http.post(`${this.apiUrl}/transfer-external`, payload);
    }
}
