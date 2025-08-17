import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ResetpassService {
    private apiUrl = 'https://localhost:7253/api/Auth/reset-password';

    constructor(private http: HttpClient) { }

    resetPassword(username: string): Observable<any> {
        return this.http.post(this.apiUrl, { username });
    }
}
