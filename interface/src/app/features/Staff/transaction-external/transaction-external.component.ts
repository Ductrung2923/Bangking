import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { TransactionExternalService, AccountDTO, ExternalBank, ExternalAccountDTO, TransferExternalPayload } from './transaction-external.services';

@Component({
  selector: 'app-transaction-external',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './transaction-external.component.html',
  styleUrls: ['./transaction-external.component.css'],
  encapsulation: ViewEncapsulation.None,
  providers: [TransactionExternalService]
})
export class TransactionExternalComponent implements OnInit {
  accountNumberInput: string = '';
  sourceAccountInfo?: AccountDTO;
  errorMessage: string = '';

  bankList: ExternalBank[] = [];
  selectedBankId: number | null = null;
  externalAccountNumber: string = '';
  externalAccountInfo?: ExternalAccountDTO;
  externalErrorMessage: string = '';

  amount: number | null = null;
  description: string = '';
  transactionMessage: string = '';

  constructor(private transactionService: TransactionExternalService) { }

  getStaffId(): number {
    return Number(sessionStorage.getItem('userId')) || 0;
  }

  ngOnInit(): void {
    this.loadExternalBanks();
  }

  loadExternalBanks() {
    this.transactionService.getAllExternalBanks().subscribe({
      next: (banks) => (this.bankList = banks),
      error: (err) => console.error('Lỗi khi tải danh sách ngân hàng:', err)
    });
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


  searchExternalAccount() {
    this.externalAccountInfo = undefined;
    this.externalErrorMessage = '';

    if (!this.selectedBankId || !this.externalAccountNumber.trim()) {
      this.externalErrorMessage = 'Vui lòng chọn ngân hàng và nhập số tài khoản đích.';
      return;
    }

    this.transactionService
      .checkExternalAccount(this.selectedBankId, this.externalAccountNumber.trim())
      .subscribe({
        next: (data) => {
          if (data.isFound) {
            this.externalAccountInfo = data;
          } else {
            this.externalErrorMessage = 'Không tìm thấy tài khoản ngoài.';
          }
        },
        error: (err) => {
          console.error('Lỗi kiểm tra tài khoản ngoài:', err);
          this.externalErrorMessage = err?.error?.message || 'Không tìm thấy tài khoản ngoài.';
        }
      });
  }


  transferMoney() {
    this.transactionMessage = '';

    if (!this.sourceAccountInfo) {
      this.transactionMessage = 'Vui lòng tìm và chọn tài khoản nguồn.';
      return;
    }

    if (!this.externalAccountInfo || !this.selectedBankId) {
      this.transactionMessage = 'Vui lòng chọn ngân hàng và nhập tài khoản đích hợp lệ.';
      return;
    }

    if (!this.amount || this.amount <= 0) {
      this.transactionMessage = 'Vui lòng nhập số tiền hợp lệ.';
      return;
    }

    const payload: TransferExternalPayload = {
      fromAccountNumber: this.sourceAccountInfo.accountNumber,
      toAccountNumber: this.externalAccountNumber.trim(),
      externalBankID: this.selectedBankId,
      amount: this.amount,
      type: 'ExternalTransfer',
      description: this.description || 'Chuyển khoản ra ngoài',
      createdAt: new Date().toISOString(),
      status: 'Success',
      staffId: this.getStaffId()
    };


    this.transactionService.transferExternal(payload).subscribe({
      next: () => {
        this.transactionMessage = `✅ Đã chuyển ${this.amount!.toLocaleString()} VND đến ${this.externalAccountInfo!.accountHolderName}`;
        // reset lại nếu cần:
        this.amount = null;
        this.description = '';
        this.externalAccountNumber = '';
        this.externalAccountInfo = undefined;
        this.selectedBankId = null;
      },
      error: (err) => {
        console.error('Lỗi chuyển khoản:', err);
        this.transactionMessage = err?.error?.message || '❌ Giao dịch thất bại.';
      }
    });
  }


  goBack() {
    history.back();
  }




}











