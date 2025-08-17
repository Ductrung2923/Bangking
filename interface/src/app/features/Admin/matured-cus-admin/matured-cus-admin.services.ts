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
    withdrawn: boolean;
}


@Injectable({
    providedIn: 'root'
})
export class MaturedAdminService {
    private apiUrl = 'https://localhost:7253/api/Transaction';

    constructor(private http: HttpClient) { }


    getMaturedSavings(): Observable<ViewSavingDTO[]> {
        return this.http.get<ViewSavingDTO[]>(`${this.apiUrl}/matured-savings`);
    }

}
