import { Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { TransactionService, AccountDTO } from './transaction-internal.services';

@Component({
  selector: 'app-transaction-internal',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './transaction-internal.component.html',
  styleUrls: ['./transaction-internal.component.css'],
  encapsulation: ViewEncapsulation.None,
  providers: [TransactionService]
})
export class TransactionInternalComponent {
  accountNumberInput: string = '';
  sourceAccountInfo?: AccountDTO;
  errorMessage: string = '';

  destinationAccountInput: string = '';
  destinationAccountInfo?: AccountDTO;
  destinationErrorMessage: string = '';

  amount: number | null = null;
  description: string = '';
  transactionMessage: string = '';
  constructor(private transactionService: TransactionService) { }

  getStaffId(): number {
    return Number(sessionStorage.getItem('userId')) || 0;
  }

  searchAccount() {
    const input = this.accountNumberInput.trim();
    this.errorMessage = '';
    this.sourceAccountInfo = undefined;

    if (input) {
      this.transactionService.checkAccount(input).subscribe({
        next: (data) => {
          if (data && data.accountNumber) {
            this.sourceAccountInfo = data;
          } else {
            this.errorMessage = 'Không tìm thấy tài khoản (dữ liệu rỗng).';
          }
        },
        error: (err) => {
          console.error('Lỗi từ server:', err);

          // ✅ Nếu backend trả về message chi tiết
          this.errorMessage =
            err?.error?.message || 'Không tìm thấy tài khoản. Vui lòng kiểm tra lại.';
        }
      });
    } else {
      this.errorMessage = 'Vui lòng nhập số tài khoản.';
    }
  }


  searchDestinationAccount() {
    const input = this.destinationAccountInput.trim();
    this.destinationErrorMessage = '';
    this.destinationAccountInfo = undefined;

    if (!input) {
      this.destinationErrorMessage = 'Vui lòng nhập số tài khoản thụ hưởng.';
      return;
    }

    if (input === this.accountNumberInput.trim()) {
      this.destinationErrorMessage = 'Số tài khoản thụ hưởng không được trùng với tài khoản nguồn.';
      return;
    }

    this.transactionService.checkAccount(input).subscribe({
      next: (data) => {
        if (data?.accountNumber) {
          this.destinationAccountInfo = data;
        } else {
          this.destinationErrorMessage = 'Không tìm thấy tài khoản thụ hưởng.';
        }
      },
      error: (err) => {
        console.error('Lỗi khi tìm tài khoản thụ hưởng:', err);
        this.destinationErrorMessage = err?.error?.message || 'Không tìm thấy tài khoản thụ hưởng.';
      }
    });
  }


  transferMoney() {
    this.transactionMessage = '';

    if (!this.sourceAccountInfo || !this.destinationAccountInfo) {
      this.transactionMessage = 'Vui lòng chọn đầy đủ tài khoản nguồn và tài khoản thụ hưởng.';
      return;
    }

    if (!this.amount || this.amount <= 0) {
      this.transactionMessage = 'Vui lòng nhập số tiền hợp lệ.';
      return;
    }

    const payload = {
      fromAccountNumber: this.sourceAccountInfo.accountNumber,
      toAccountNumber: this.destinationAccountInfo.accountNumber,
      amount: this.amount,
      type: 'Transfer',
      description: this.description || 'Chuyển tiền nội bộ',
      createdAt: new Date().toISOString(),
      status: 'Success',
      staffId: this.getStaffId()
    };

    this.transactionService.transferInternal(payload).subscribe({
      next: () => {
        this.transactionMessage = `✅ Đã chuyển ${this.amount!.toLocaleString()} VND đến tài khoản ...`;
        // Reset lại nếu muốn
        this.amount = null;
        this.description = '';
      },
      error: (err) => {
        console.error('Lỗi chuyển tiền:', err);
        this.transactionMessage = err?.error?.message || '❌ Giao dịch thất bại.';
      }
    });
  }


  goBack() {
    history.back();
  }




}











