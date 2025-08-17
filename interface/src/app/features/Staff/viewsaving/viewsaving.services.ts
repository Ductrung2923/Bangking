import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ViewSavingDTO {
    savingID: number;
    customerID: number;
    customerName: string;
    photoUrl: string;
    accountID: number;
    accountNumber: string;
    amount: number;
    interestRate: number;
    termMonths: number;
    startDate: string;
    endDate: string;
    receiveInterestMethod: string;
    status: string;
    totalInterestEarned: number;
}


@Injectable({
    providedIn: 'root'
})
export class ViewsavingService {
    private apiUrl = 'https://localhost:7253/api/Transaction';

    constructor(private http: HttpClient) { }


    getAllSavings(): Observable<ViewSavingDTO[]> {
        return this.http.get<ViewSavingDTO[]>(`${this.apiUrl}/View-Saving`);
    }


    calculateInterest(): Observable<string> {
        return this.http.post(`${this.apiUrl}/calculate-interest`, {}, { responseType: 'text' });
    }
}
