import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface RegisterRequest {
    username: string;
    password: string;
    email: string;
    fullName: string;
    employeeCode: string;
    gender: boolean;
    dateOfBirth: string;
    hireDate: string;
    photoUrl: string;
    position: string;
    department: string;
    notes: string;
}

@Injectable({
    providedIn: 'root'
})
export class RegisterService {
    private apiStep1Url = 'https://localhost:7253/api/Register/step1'; // Điều chỉnh URL theo API .NET của bạn
    private apiStep2Url = 'https://localhost:7253/api/Register/step2'; // Xác thực OTP
    constructor(private http: HttpClient) { }

    /** Bước 1: Gửi thông tin đăng ký */
    registerStep1(data: RegisterRequest): Observable<any> {
        return this.http.post(this.apiStep1Url, data);
    }

    /** Bước 2: Xác nhận OTP */
    verifyOTP(email: string, otp: string): Observable<any> {
        return this.http.post(this.apiStep2Url, { email, otp });
    }
}
