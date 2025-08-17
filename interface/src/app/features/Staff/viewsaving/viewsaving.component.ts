import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ViewsavingService, ViewSavingDTO } from './viewsaving.services';

@Component({
  selector: 'app-viewsaving',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './viewsaving.component.html',
  styleUrls: ['./viewsaving.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ViewsavingComponent implements OnInit {
  savings: ViewSavingDTO[] = [];
  isLoading = false;
  message: string | null = null;


  constructor(private viewsaving: ViewsavingService) { }

  ngOnInit(): void {
    this.loadSavings();
  }

  loadSavings() {
    this.isLoading = true;
    this.viewsaving.getAllSavings().subscribe({
      next: data => {
        this.savings = data;
        this.isLoading = false;
      },
      error: err => {
        console.error('Lỗi load sổ tiết kiệm:', err);
        this.isLoading = false;
      }
    });
  }

  onCalculateAllInterest() {

    this.viewsaving.calculateInterest().subscribe({
      next: message => {
        this.message = message;       // ✔ Thông báo thành công
        setTimeout(() => {
          this.loadSavings(); // Gọi lại sau 1-2s
        }, 100);           // ✔ Tải lại danh sách

      },
      error: err => {
        try {
          this.message = JSON.parse(err.error)?.message || 'Đã xảy ra lỗi.';
        } catch {
          this.message = 'Đã xảy ra lỗi.';
        }
        console.error(err);
      }
    });

  }




  goBack() {
    history.back();
  }

}
