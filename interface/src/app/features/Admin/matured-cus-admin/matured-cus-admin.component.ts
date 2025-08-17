import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MaturedAdminService, ViewSavingDTO } from './matured-cus-admin.services';
import { NgxPaginationModule } from 'ngx-pagination';
@Component({
  selector: 'app-matured-savings',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule, NgxPaginationModule],
  templateUrl: './matured-cus-admin.component.html',
  styleUrls: ['./matured-cus-admin.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class MaturedCusAdminComponent implements OnInit {

  p: number = 1;
  itemsPerPage: number = 2;

  maturedSavings: (ViewSavingDTO & { totalMoney: number })[] = [];
  isLoading = false;
  message: string | null = null;

  constructor(private Matured: MaturedAdminService) { }

  ngOnInit(): void {
    this.loadMaturedSavings();
  }

  loadMaturedSavings(): void {
    this.isLoading = true;
    this.Matured.getMaturedSavings().subscribe({
      next: (data) => {
        this.maturedSavings = data.map((s) => ({
          ...s,
          totalMoney: s.amount + s.totalInterestEarned,
          withdrawn: localStorage.getItem(`withdrawn_${s.savingID}`) === 'true', // đọc trạng thái lưu localStorage
        }));
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Lỗi khi load sổ đáo hạn:', err);
        this.isLoading = false;
      },
    });
  }

  goBack(): void {
    history.back();
  }
}
