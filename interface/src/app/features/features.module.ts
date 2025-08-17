import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { HomeAdComponent } from './Admin/home-ad/home-ad.component';
import { ProfileAdminComponent } from './Admin/profile-admin/profile-admin.component';
import { ProfileStaffComponent } from './Staff/profile-staff/profile-staff.component';
import { ConfirmStaffComponent } from './Admin/confirm-staff/confirm-staff.component';
@NgModule({

  imports: [
    CommonModule,
    RouterModule,

  ],

  exports: []
})
export class FeaturesModule { }