import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Location } from '@angular/common';
import { ProfileStaffService } from './profile-staff.services';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-profile-staff',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './profile-staff.component.html',
  styleUrls: ['./profile-staff.component.css']
})
export class ProfileStaffComponent implements OnInit {
  staffData: any;

  constructor(
    private location: Location,
    private profileService: ProfileStaffService
  ) { }

  ngOnInit(): void {
    this.profileService.getStaffProfile().subscribe({
      next: (data) => {
        this.staffData = data;
        console.log('Staff data:', data);
      },
      error: (err) => {
        console.error('Lỗi khi lấy thông tin staff:', err);
      }
    });
  }

  onEdit(): void {
    this.profileService.updateStaffProfile(this.staffData).subscribe({
      next: (res) => {
        sessionStorage.setItem('fullName', this.staffData.fullName);
        sessionStorage.setItem('photoUrl', this.staffData.photoUrl);
        alert('Cập nhật thành công!');
      },
      error: (err) => {
        console.error('Lỗi khi cập nhật:', err);
        alert('Cập nhật thất bại!');
      }
    })
  }

  goBack() {
    history.back();
  }
}
