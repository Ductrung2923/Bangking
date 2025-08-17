import { Component, OnInit } from '@angular/core';
import { ListcusStaffService, CustomerDTO } from './liscus-staff.services';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-listcus-staff',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './listcus-staff.component.html',
  styleUrls: ['./listcus-staff.component.css']
})
export class ListcusStaffComponent implements OnInit {

  customers: CustomerDTO[] = [];
  showAddForm: boolean = false;

  newCustomer: any = {
    fullName: '',
    email: '',
    phone: '',
    address: '',
    identityNumber: '',
    dateOfBirth: '',
    cardNumber: '',
    balance: 0,
    photoUrl: '',
    accountNumber: '',
    gender: true,
    createdAt: new Date().toISOString(),
    createdAtAc: new Date().toISOString()
  };

  message: string = '';
  messageType: 'success' | 'error' | '' = '';

  constructor(private listcusService: ListcusStaffService) { }

  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers() {
    this.listcusService.getAllCustomers().subscribe({
      next: (data) => {
        this.customers = data;
      },
      error: (err) => {
        console.error('Lỗi khi tải danh sách khách hàng:', err);
      }
    });
  }

  openAddCustomer() {
    this.showAddForm = true;
  }

  closeAddCustomer() {
    this.showAddForm = false;
    this.resetForm();
  }

  submitCustomer() {
    console.log("Đang gửi:", this.newCustomer);

    this.listcusService.addCustomer(this.newCustomer).subscribe({
      next: () => {
        alert('✅ Thêm khách hàng thành công!');
        this.closeAddCustomer();
        this.loadCustomers();
      },
      error: (err) => {
        console.error('❌ Lỗi khi thêm khách hàng:', err);
        alert('❌ Thêm khách hàng thất bại!');
      }
    });
  }


  resetForm() {
    this.newCustomer = {
      fullName: '',
      email: '',
      phone: '',
      address: '',
      identityNumber: '',
      dateOfBirth: '',
      cardNumber: '',
      balance: 0,
      photoUrl: '',
      accountNumber: '',
      gender: true,
      createdAt: new Date().toISOString(),
      createdAtAc: new Date().toISOString()
    };
  }



  goBack() {
    history.back();
  }





}