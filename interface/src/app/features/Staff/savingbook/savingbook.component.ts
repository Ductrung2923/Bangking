import { Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { SavingsbookService, AccountDTO } from './savingbook.services';
@Component({
  selector: 'app-savingbook',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './savingbook.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./savingbook.component.css'],
})
export class SavingbookComponent {
  accountNumberInput: string = '';
  sourceAccountInfo?: AccountDTO;
  errorMessage: string = '';


  saving = {
    amount: 0,
    interestRate: 5,
    termMonths: 3,
    receiveInterestMethod: 'ToAccount'
  };

  message: string = '';
  constructor(private savingbook: SavingsbookService) { }

  getStaffId(): number {
    return Number(sessionStorage.getItem('userId')) || 0;
  }

  searchAccount() {
    const input = this.accountNumberInput.trim();
    this.errorMessage = '';
    this.sourceAccountInfo = undefined;

    if (input) {
      this.savingbook.checkAccount(input).subscribe({
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


  createSaving() {
    if (!this.sourceAccountInfo) return;

    const dto = {
      customerId: this.sourceAccountInfo.customerID || 0,
      accountNumber: this.sourceAccountInfo.accountNumber,
      amount: this.saving.amount,
      interestRate: this.saving.interestRate,
      termMonths: this.saving.termMonths,
      receiveInterestMethod: this.saving.receiveInterestMethod,
      staffId: this.getStaffId()
    };

    this.savingbook.createSaving(dto).subscribe({
      next: () => {
        this.message = '✅ Tạo sổ tiết kiệm thành công!';
        this.saving.amount = 0;
      },
      error: (err: any) => {
        this.message = err?.error?.message || '❌ Gửi tiết kiệm thất bại!';
      }
    });
  }


  goBack() {
    history.back();
  }

}
