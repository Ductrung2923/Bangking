// confirm-staff.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { ConfirmStaffService, StaffDTO } from './confirm-staff.services';

@Component({
  selector: 'app-confirm-staff',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule, NgxPaginationModule],
  templateUrl: './confirm-staff.component.html',
  styleUrls: ['./confirm-staff.component.css']
})
export class ConfirmStaffComponent implements OnInit {
  p: number = 1;
  itemsPerPage: number = 4;

  staffList: StaffDTO[] = [];
  loading: boolean = false;

  constructor(private confirmStaffService: ConfirmStaffService) { }

  ngOnInit(): void {
    this.loadStaff();
  }

  loadStaff(): void {
    this.loading = true;
    this.confirmStaffService.getAllStaff().subscribe({
      next: (data) => {
        this.staffList = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load staff', err);
        this.loading = false;
      }
    });
  }

  toggleApproval(userId: number, currentStatus: boolean | null | undefined): void {
    const newStatus = currentStatus === true ? false : true; // an toàn với null/undefined
    this.confirmStaffService.updateStaffStatus(userId, newStatus).subscribe({
      next: () => {
        const staff = this.staffList.find(s => s.userId === userId);
        if (staff) staff.isActive = newStatus;
      },
      error: (err) => {
        console.error('Update failed', err);
      }
    });
  }

  goBack() {
    history.back();
  }
}  
