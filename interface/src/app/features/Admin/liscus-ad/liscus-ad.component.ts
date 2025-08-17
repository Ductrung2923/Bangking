import { Component, OnInit } from '@angular/core';
import { ListcusAdminService, CustomerDTO } from './liscus-ad.services';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { NgxPaginationModule } from 'ngx-pagination';

@Component({
  selector: 'app-listcus-admin',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, NgxPaginationModule],
  templateUrl: './liscus-ad.component.html',
  styleUrls: ['./liscus-ad.component.css']
})
export class ListcusAdminComponent implements OnInit {

  p: number = 1;
  itemsPerPage: number = 4;

  customers: CustomerDTO[] = [];

  constructor(private listcusService: ListcusAdminService) { }

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

  goBack() {
    history.back();
  }

}
