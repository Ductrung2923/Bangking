import { Component, OnInit, ViewEncapsulation, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService, LoginRequest } from '../login/login.service';
import { RegisterService, RegisterRequest } from './Register.service';
import { ResetpassService } from './Resetpass.services';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgIf } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { UserProfileService } from '../../core/user-profile.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [FormsModule, HttpClientModule, NgIf],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {
  container: HTMLElement | null = null;
  username: string = '';
  password: string = '';
  errorMessage: string = '';
  registerErrorMessage: string = '';
  registerSuccessMessage: string = '';
  otp: string = '';
  showOtpPopup: boolean = false;
  isLoginMode: boolean = true;

  // ✅ Forgot password
  showForgotPasswordPopup: boolean = false;
  forgotPasswordUsername: string = '';
  forgotPasswordError: string = '';
  forgotPasswordSuccess: string = '';

  fieldErrors: { [key: string]: string } = {};

  registerData: RegisterRequest = {
    username: '',
    password: '',
    email: '',
    fullName: '',
    employeeCode: '',
    position: '',
    department: '',
    gender: true,
    dateOfBirth: '',
    hireDate: '',
    photoUrl: '',
    notes: ''
  };

  constructor(
    private loginService: LoginService,
    private registerService: RegisterService,
    private resetpassService: ResetpassService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private userProfileService: UserProfileService
  ) { }

  ngOnInit(): void {
    setTimeout(() => {
      this.container = document.getElementById('container');
      if (this.container) {
        this.container.classList.add('sign-in');
      }
    }, 0);
  }

  toggle(): void {
    if (this.container) {
      this.container.classList.toggle('sign-in');
      this.container.classList.toggle('sign-up');
      this.isLoginMode = !this.isLoginMode;
      this.registerErrorMessage = '';
      this.registerSuccessMessage = '';
      this.fieldErrors = {};
    }
  }

  login(): void {
    const loginData: LoginRequest = { username: this.username, password: this.password };
    this.loginService.login(loginData).subscribe(
      response => {
        const token = response.token;
        sessionStorage.setItem('token', token);

        try {
          const decoded: any = jwtDecode(token);
          const role = decoded["role"] || decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
          const userId = decoded["sub"] || decoded["userId"];



          sessionStorage.setItem('role', role);
          sessionStorage.setItem('userId', userId);


          this.userProfileService.loadAndStoreUserProfile(role, token);


          setTimeout(() => {
            if (role == '1' || role == 1) {
              this.router.navigate(['/home-ad']);
            } else if (role == '0' || role == 0) {
              this.router.navigate(['/home-staff']);
            }
          }, 300);
        } catch (e) {
          console.error('Token không hợp lệ:', e);
          this.errorMessage = 'Token không hợp lệ.';
        }
      },
      error => {
        console.error('Đăng nhập thất bại:', error);
        this.errorMessage = error?.error?.message || 'Thông tin đăng nhập không đúng. Vui lòng thử lại.';
        this.cdr.detectChanges();
      }
    );
  }


  private formatDate(dateStr: string): string {
    const date = new Date(dateStr);
    if (isNaN(date.getTime())) return '';
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
  }


  registerStep1(): void {
    this.registerErrorMessage = '';
    this.registerSuccessMessage = '';
    this.fieldErrors = {};

    if (!this.registerData.username || !this.registerData.email || !this.registerData.password) {
      this.registerErrorMessage = 'Vui lòng nhập Username, Email và Password.';
      return;
    }

    const formattedDob = this.formatDate(this.registerData.dateOfBirth);
    const formattedHireDate = this.formatDate(this.registerData.hireDate);

    const payload: RegisterRequest = {
      ...this.registerData,
      dateOfBirth: formattedDob,
      hireDate: formattedHireDate,
    };

    this.registerService.registerStep1(payload).subscribe({
      next: resp => {
        if (resp?.message?.includes('OTP')) {
          this.showOtpPopup = true;
        } else {
          this.registerErrorMessage = `Gửi OTP thất bại: ${JSON.stringify(resp)}`;
        }
      },
      error: err => {
        console.error('❌ Lỗi API:', err);
        this.fieldErrors = {};
        if (err?.error?.errors) {
          for (const key in err.error.errors) {
            if (err.error.errors.hasOwnProperty(key)) {
              const message = err.error.errors[key][0];
              this.fieldErrors[key.toLowerCase()] = message;
            }
          }
        }
        this.registerErrorMessage = err?.error?.title || 'Không thể gửi OTP. Vui lòng kiểm tra lại thông tin.';
      }
    });
  }

  verifyOTP(): void {
    if (!this.otp || !this.registerData.email) {
      this.registerErrorMessage = 'Vui lòng nhập OTP và Email.';
      this.cdr.detectChanges();
      return;
    }

    this.registerService.verifyOTP(this.registerData.email, this.otp).subscribe(
      response => {
        console.log('Xác nhận OTP thành công:', response);
        this.closeOtpPopup();

        this.registerData = {
          username: '',
          password: '',
          email: '',
          fullName: '',
          employeeCode: '',
          position: '',
          department: '',
          gender: true,
          dateOfBirth: '',
          hireDate: '',
          photoUrl: '',
          notes: ''
        };

        this.registerSuccessMessage = '🎉 Tạo tài khoản thành công!';
        this.otp = '';
        this.fieldErrors = {};
        this.cdr.detectChanges();

        setTimeout(() => {
          if (this.container) {
            this.container.classList.remove('sign-up');
            this.container.classList.add('sign-in');
          }
          this.isLoginMode = true;
          this.registerSuccessMessage = '';
          this.cdr.detectChanges();
        }, 2500);
      },
      error => {
        console.error('Xác nhận OTP thất bại:', error);
        this.registerErrorMessage = error.error?.message || 'OTP không đúng. Vui lòng thử lại.';
        this.cdr.detectChanges();
      }
    );
  }

  closeOtpPopup(): void {
    this.showOtpPopup = false;
    this.otp = '';
    this.registerErrorMessage = '';
    this.cdr.detectChanges();
  }

  // ✅ FORGOT PASSWORD HANDLERS
  openForgotPasswordPopup(): void {
    this.forgotPasswordUsername = '';
    this.forgotPasswordError = '';
    this.forgotPasswordSuccess = '';
    this.showForgotPasswordPopup = true;
  }

  closeForgotPasswordPopup(): void {
    this.showForgotPasswordPopup = false;
    this.forgotPasswordUsername = '';
    this.forgotPasswordError = '';
    this.forgotPasswordSuccess = '';
  }

  resetPassword(): void {
    if (!this.forgotPasswordUsername) {
      this.forgotPasswordError = 'Vui lòng nhập tên đăng nhập.';
      this.forgotPasswordSuccess = '';
      return;
    }

    this.resetpassService.resetPassword(this.forgotPasswordUsername).subscribe({
      next: () => {
        this.forgotPasswordSuccess = '✅ Mật khẩu mới đã được gửi đến email của bạn.';
        this.forgotPasswordError = '';
      },
      error: (err) => {
        console.error('Lỗi reset mật khẩu:', err);
        this.forgotPasswordError = err?.error?.message || 'Đã xảy ra lỗi. Vui lòng thử lại.';
        this.forgotPasswordSuccess = '';
      }
    });
  }
}
