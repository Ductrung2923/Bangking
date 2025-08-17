import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { NgChartsModule } from 'ng2-charts';
import { ChartConfiguration } from 'chart.js';
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';

import {
  HomeAdService,
  CustomerDTO,
  TransactionHistoryDTO,
  WeeklyTransactionStatDTO
} from './home-ad.services';

@Component({
  selector: 'app-home-ad',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, NgChartsModule],
  templateUrl: './home-ad.component.html',
  styleUrls: ['./home-ad.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class HomeAdComponent implements OnInit {
  // Dashboard values
  totalStaff: number = 0;
  totalCustomers: number = 0;
  totalAccounts: number = 0;
  totalExternalBanks: number = 0;
  totalBankBalance: number = 0;

  // Chart data (updated dynamically from backend)
  barChartLabels: string[] = [];
  barChartData: ChartConfiguration<'bar', number[], string>['data'] = {
    labels: this.barChartLabels,
    datasets: [
      { data: [], label: 'Transactions' }
    ]
  };
  barChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
    scales: {
      y: {
        beginAtZero: true,
        max: 15, // ✅ Gán thủ công trục Y tối đa là 15
        ticks: {
          stepSize: 3 // ✅ Hiển thị mỗi 3 đơn vị (hoặc 5, 1, tuỳ bạn)
        },
        title: {
          display: true,
          text: 'Số lượng giao dịch'
        }
      },
      x: {
        title: {
          display: true,
          text: 'Thứ trong tuần'
        }
      }
    },
    plugins: {
      legend: {
        display: true,
        position: 'top'
      }
    }
  };

  barChartType: 'bar' = 'bar'; //

  // Other properties
  recentCustomers: CustomerDTO[] = [];
  transactionHistory: TransactionHistoryDTO[] = [];
  weeklyStats: WeeklyTransactionStatDTO[] = [];
  userFullName: string = 'Admin';
  userPhotoUrl: string = 'https://i.pravatar.cc/150?img=3';
  dashboardOpen = false;

  constructor(private homeAdService: HomeAdService) { }

  ngOnInit(): void {
    const fullName = sessionStorage.getItem('fullName');
    const photoUrl = sessionStorage.getItem('photoUrl');

    if (fullName) this.userFullName = fullName;
    if (photoUrl) this.userPhotoUrl = photoUrl;

    this.fetchDashboardStats();
    this.fetchRecentCustomers();
    this.fetchTransactionHistory();
    this.fetchWeeklyStats(); // 🔥 Lấy biểu đồ giao dịch tuần
  }

  toggleDashboard() {
    this.dashboardOpen = !this.dashboardOpen;
  }

  fetchRecentCustomers() {
    this.homeAdService.getRecentCustomers().subscribe({
      next: (data) => (this.recentCustomers = data),
      error: (err) => console.error('❌ Lỗi khi tải khách hàng mới nhất:', err),
    });
  }

  fetchTransactionHistory() {
    this.homeAdService.getTransactionHistory().subscribe({
      next: (data) => (this.transactionHistory = data),
      error: (err) => console.error('❌ Lỗi khi tải lịch sử giao dịch:', err),
    });
  }

  fetchDashboardStats() {
    this.homeAdService.getTotalStaff().subscribe({
      next: (data) => (this.totalStaff = data),
      error: (err) => console.error('❌ Lỗi lấy tổng nhân viên:', err),
    });

    this.homeAdService.getTotalCustomers().subscribe({
      next: (data) => (this.totalCustomers = data),
      error: (err) => console.error('❌ Lỗi lấy tổng khách hàng:', err),
    });

    this.homeAdService.getTotalAccounts().subscribe({
      next: (data) => (this.totalAccounts = data),
      error: (err) => console.error('❌ Lỗi lấy tổng tài khoản:', err),
    });

    this.homeAdService.getTotalExternalBanks().subscribe({
      next: (data) => (this.totalExternalBanks = data),
      error: (err) => console.error('❌ Lỗi lấy tổng ngân hàng ngoài:', err),
    });

    this.homeAdService.getTotalBankBalance().subscribe({
      next: (res) => {
        console.log('✅ Tổng số dư trả về:', res);  // Thêm dòng này
        this.totalBankBalance = res.totalBalance;
      },
      error: (err) => console.error('❌ Lỗi lấy tổng số dư ngân hàng:', err),
    });


  }

  fetchWeeklyStats() {
    this.homeAdService.getWeeklyTransactionStats().subscribe({
      next: (data) => {
        this.weeklyStats = data;

        const orderedDays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
        const countsMap: { [key: string]: number } = {};

        // Đổ dữ liệu vào map
        data.forEach(d => countsMap[d.day] = d.count);

        // Đảm bảo đúng thứ tự + đủ 7 ngày
        this.barChartLabels = orderedDays;
        const counts = orderedDays.map(day => countsMap[day] || 0);

        // Gán vào chart
        this.barChartData = {
          labels: this.barChartLabels,
          datasets: [
            {
              data: counts,
              label: 'Transactions',
              backgroundColor: 'rgba(54, 162, 235, 0.5)',
              borderColor: 'rgba(54, 162, 235, 1)',
              borderWidth: 1
            }
          ]
        };
      },
      error: (err) => console.error('❌ Lỗi lấy thống kê tuần:', err),
    });
  }

  exportToExcel(data: any[], fileName: string): void {
    const worksheet = XLSX.utils.json_to_sheet(data);
    const workbook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelBuffer], { type: 'application/octet-stream' });
    saveAs(blob, `${fileName}.xlsx`);
  }

}
