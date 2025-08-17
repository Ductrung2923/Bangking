import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable({
    providedIn: 'root'
})
export class UserProfileService {
    constructor(private http: HttpClient) { }

    loadAndStoreUserProfile(role: string, token: string): void {
        const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
        let url = '';

        switch (role) {
            case '1':
                url = 'https://localhost:7253/api/ProfileAdmin/admin';
                break;
            case '0':
                url = 'https://localhost:7253/api/ProfileStaff/staff';
                break;
            default:
                console.warn('Role không hợp lệ:', role);
                return;
        }

        this.http.get<any>(url, { headers }).subscribe({
            next: (data) => {
                const fullName = data?.user?.fullName || data?.fullName || '';
                const photoUrl = data?.photoUrl || 'https://i.pravatar.cc/150?img=3';
                sessionStorage.setItem('fullName', fullName); // đổi sang sessionStorage
                sessionStorage.setItem('photoUrl', photoUrl); // đổi sang sessionStorage
            },
            error: (err) => console.error('Lỗi lấy profile:', err)
        });
    }
}
