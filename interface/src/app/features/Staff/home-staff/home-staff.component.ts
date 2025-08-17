import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { HomeStaffService, CustomerDTO, TransactionHistoryDTO } from './home-staff.services';

@Component({
  selector: 'app-home-staff',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule],
  templateUrl: './home-staff.component.html',
  styleUrl: './home-staff.component.css'
})
export class HomeStaffComponent implements OnInit {

  dashboardOpen = false;
  userFullName: string = 'Staff';
  userPhotoUrl: string = 'https://i.pravatar.cc/150?img=3';

  recentCustomers: CustomerDTO[] = [];
  transactionHistory: TransactionHistoryDTO[] = [];

  constructor(private homeStaffService: HomeStaffService) { }

  ngOnInit(): void {
    const fullName = sessionStorage.getItem('fullName');
    const photoUrl = sessionStorage.getItem('photoUrl');

    if (fullName) {
      this.userFullName = fullName;
    }

    if (photoUrl) {
      this.userPhotoUrl = photoUrl;
    }

    this.fetchRecentCustomers();
    this.fetchTransactionHistory();
  }

  toggleDashboard() {
    this.dashboardOpen = !this.dashboardOpen;
  }

  fetchRecentCustomers() {
    this.homeStaffService.getRecentCustomers().subscribe({
      next: (data) => {
        this.recentCustomers = data;
      },
      error: (err) => {
        console.error('❌ Lỗi khi tải khách hàng mới nhất:', err);
      }
    });
  }

  fetchTransactionHistory() {
    this.homeStaffService.getTransactionHistory().subscribe({
      next: (data) => {
        this.transactionHistory = data;
      },
      error: (err) => {
        console.error('❌ Lỗi khi tải lịch sử giao dịch:', err);
      }
    });
  }
}
