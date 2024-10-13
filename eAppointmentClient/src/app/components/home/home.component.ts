import { Component } from '@angular/core';
import { departments } from '../../constant';
import { DoctorModel } from '../../models/doctor.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DxSchedulerModule } from 'devextreme-angular';
import { HttpService } from '../../services/http.service';
import { HttpXsrfTokenExtractor } from '@angular/common/http';
import { AppointmentModel } from '../../models/appointment.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule, CommonModule, DxSchedulerModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  departments = departments;
  doctors: DoctorModel[] = [];

  selectedDepartmentValue: number = 0;
  selectedDoctorId: string = '';

  appointments: AppointmentModel[] = [];

  constructor(private http: HttpService) {}

  getAllDoctorsByDepartment() {
    this.selectedDoctorId = '';
    this.http.post<DoctorModel[]>(
      'Appointments/GetAllDoctorsByDepartment',
      { departmentValue: +this.selectedDepartmentValue },
      (res) => {
        this.doctors = res.data;
      }
    );
  }

  getAllAppointmentsByDoctorId() {
    if (this.selectedDoctorId) {
      this.http.post<AppointmentModel[]>(
        'Appointments/GetAllAppointmentsByDoctorId',
        { doctorId: this.selectedDoctorId },
        (res) => {
          this.appointments = res.data;
        }
      );
    }
  }
}
