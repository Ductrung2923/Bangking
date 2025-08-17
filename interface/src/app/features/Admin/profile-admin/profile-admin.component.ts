import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Location } from '@angular/common';
import { ProfileAdminService } from './profile-admin.services';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile-admin',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './profile-admin.component.html',
  styleUrls: ['./profile-admin.component.css']
})
export class ProfileAdminComponent implements OnInit {
  adminData: any;

  constructor(
    private location: Location,
    private profileService: ProfileAdminService
  ) { }

  ngOnInit(): void {
    this.profileService.getAdminProfile().subscribe({
      next: (data) => {
        this.adminData = data;
        console.log('Admin data:', data); // 🐞 DEBUG
      },
      error: (err) => {
        console.error('Lỗi khi lấy thông tin admin:', err);
      }
    });
  }

  onEdit(): void {
    this.profileService.updateAdminProfile(this.adminData).subscribe({
      next: (res) => {
        sessionStorage.setItem('fullName', this.adminData.fullName);
        sessionStorage.setItem('photoUrl', this.adminData.photoUrl);
        alert('Cập nhật thành công!');

      },
      error: (err) => {
        console.error('Lỗi khi cập nhật:', err);
        alert('Cập nhật thất bại!');
      }
    });
  }


  goBack() {
    history.back();
  }
}
