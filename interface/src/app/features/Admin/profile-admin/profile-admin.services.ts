import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ProfileAdminService {
    constructor(private http: HttpClient) { }

    getAdminProfile(): Observable<any> {
        const token = sessionStorage.getItem('token'); // <-- Đọc từ sessionStorage
        const headers = new HttpHeaders({
            Authorization: `Bearer ${token}`
        });

        return this.http.get<any>('https://localhost:7253/api/ProfileAdmin/admin', { headers });
    }

    updateAdminProfile(admin: any): Observable<any> {
        const token = sessionStorage.getItem('token');
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        });

        return this.http.put<any>('https://localhost:7253/api/UpdateProfileAdmin', admin, { headers });
    }

}
