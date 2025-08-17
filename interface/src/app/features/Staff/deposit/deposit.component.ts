import { Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { DepositService, AccountDTO, DepositDTO } from './deposit.services';

@Component({
  selector: 'app-deposit',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './deposit.component.html',
  styleUrls: ['./deposit.component.css'],
  encapsulation: ViewEncapsulation.None,
  providers: [DepositService]
})
export class DepositComponent {
  accountNumberInput: string = '';
  accountInfo?: AccountDTO;
  errorMessage: string = '';

  amount: number | null = null;
  description: string = '';
  depositMessage: string = '';

  constructor(private depositService: DepositService) { }

  getStaffId(): number {
    return Number(sessionStorage.getItem('userId')) || 0;
  }

  searchAccount() {
    const input = this.accountNumberInput.trim();
    this.errorMessage = '';
    this.accountInfo = undefined;

    if (!input) {
      this.errorMessage = 'Vui lòng nhập số tài khoản.';
      return;
    }

    this.depositService.checkAccount(input).subscribe({
      next: (data) => {
        if (data?.accountNumber) {
          this.accountInfo = data;
        } else {
          this.errorMessage = 'Không tìm thấy tài khoản (dữ liệu rỗng).';
        }
      },
      error: (err) => {
        console.error('Lỗi từ server:', err);
        this.errorMessage = err?.error?.message || 'Không tìm thấy tài khoản. Vui lòng kiểm tra lại.';
      }
    });
  }

  depositMoney() {
    this.depositMessage = '';

    if (!this.accountInfo) {
      this.depositMessage = 'Vui lòng kiểm tra và chọn tài khoản trước khi nạp tiền.';
      return;
    }

    if (!this.amount || this.amount <= 0) {
      this.depositMessage = 'Vui lòng nhập số tiền hợp lệ.';
      return;
    }

    const payload: DepositDTO = {
      accountNumber: this.accountInfo.accountNumber,
      amount: this.amount,
      description: this.description || 'Nạp tiền vào tài khoản',
      staffId: this.getStaffId()
    };

    this.depositService.deposit(payload).subscribe({
      next: () => {
        this.depositMessage = `✅ Đã nạp ${this.amount!.toLocaleString()} VND vào tài khoản.`;
        this.amount = null;
        this.description = '';
      },
      error: (err) => {
        console.error('Lỗi khi nạp tiền:', err);
        this.depositMessage = err?.error?.message || '❌ Nạp tiền thất bại.';
      }
    });
  }

  goBack() {
    history.back();
  }
}
