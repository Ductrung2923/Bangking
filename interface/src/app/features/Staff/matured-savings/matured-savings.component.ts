import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MaturedService, ViewSavingDTO } from './matured-savings.services';

@Component({
  selector: 'app-matured-savings',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './matured-savings.component.html',
  styleUrls: ['./matured-savings.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class MaturedSavingsComponent implements OnInit {
  maturedSavings: (ViewSavingDTO & { totalMoney: number })[] = [];
  isLoading = false;
  message: string | null = null;

  constructor(private Matured: MaturedService) { }

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

  handleCheckboxChange(savingID: number, event: Event): void {
    const checked = (event.target as HTMLInputElement).checked;

    if (!checked) {
      // Nếu người dùng cố gắng bỏ tích, chặn lại (reset checkbox thành true)
      const checkbox = event.target as HTMLInputElement;
      checkbox.checked = true;
      return;
    }

    // Cập nhật trạng thái
    const index = this.maturedSavings.findIndex((s) => s.savingID === savingID);
    if (index >= 0) {
      this.maturedSavings[index].withdrawn = true;
      localStorage.setItem(`withdrawn_${savingID}`, 'true');
    }
  }

  goBack(): void {
    history.back();
  }
}
