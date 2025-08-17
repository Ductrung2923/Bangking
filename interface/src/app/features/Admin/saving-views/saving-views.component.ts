import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { savingViewService, ViewSavingDTO } from './saving-view.services';
import { NgxPaginationModule } from 'ngx-pagination';
@Component({
  selector: 'app-viewsaving',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule, NgxPaginationModule],
  templateUrl: './saving-views.component.html',
  styleUrls: ['./saving-views.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class SavingViewsComponent {
  // phan trang
  p: number = 1;
  itemsPerPage: number = 3;

  savings: ViewSavingDTO[] = [];
  isLoading = false;
  message: string | null = null;


  constructor(private viewsaving: savingViewService) { }

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

  goBack() {
    history.back();
  }

}
