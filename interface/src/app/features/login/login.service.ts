
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface LoginRequest {
    username: string;
    password: string;
}

@Injectable({
    providedIn: 'root' // dùng root vì bạn đang không dùng module riêng
})
export class LoginService {
    private apiUrl = 'https://localhost:7253/api/Auth/login'; // ← chỉnh URL theo API .NET

    constructor(private http: HttpClient) { }

    login(data: LoginRequest): Observable<any> {
        return this.http.post(this.apiUrl, data);
    }
}
