import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ProfileStaffService {
    private apiUrl = 'https://localhost:7253/api/ProfileStaff/staff';

    constructor(private http: HttpClient) { }

    getStaffProfile(): Observable<any> {
        const token = sessionStorage.getItem('token');
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`
        });

        return this.http.get<any>(this.apiUrl, { headers });
    }

    updateStaffProfile(staff: any): Observable<any> {
        const token = sessionStorage.getItem('token');
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        });

        return this.http.put<any>('https://localhost:7253/api/UpdateProfileStaff', staff, { headers });
    }

}
