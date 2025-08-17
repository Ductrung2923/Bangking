import { Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { WithdrawService, AccountDTO, WithdrawDTO } from './withdraw.services';

@Component({
  selector: 'app-withdraw',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './withdraw.component.html',
  styleUrls: ['./withdraw.component.css'],
  encapsulation: ViewEncapsulation.None,
  providers: [WithdrawService]
})
export class WithdrawComponent {
  accountNumberInput: string = '';
  accountInfo?: AccountDTO;
  errorMessage: string = '';

  amount: number | null = null;
  description: string = '';
  withdrawMessage: string = '';

  constructor(private withdrawService: WithdrawService) { }

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

    this.withdrawService.checkAccount(input).subscribe({
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

  withdrawMoney() {
    this.withdrawMessage = '';

    if (!this.accountInfo) {
      this.withdrawMessage = 'Vui lòng kiểm tra và chọn tài khoản trước khi rút tiền.';
      return;
    }

    if (!this.amount || this.amount <= 0) {
      this.withdrawMessage = 'Vui lòng nhập số tiền hợp lệ.';
      return;
    }

    const payload: WithdrawDTO = {
      accountNumber: this.accountInfo.accountNumber,
      amount: this.amount,
      description: this.description || 'Rút tiền từ tài khoản',
      staffId: this.getStaffId()
    };

    this.withdrawService.withdraw(payload).subscribe({
      next: () => {
        this.withdrawMessage = `✅ Đã rút ${this.amount!.toLocaleString()} VND khỏi tài khoản.`;
        this.amount = null;
        this.description = '';
      },
      error: (err) => {
        console.error('Lỗi khi rút tiền:', err);
        this.withdrawMessage = err?.error?.message || '❌ Rút tiền thất bại.';
      }
    });
  }

  goBack() {
    history.back();
  }
}
