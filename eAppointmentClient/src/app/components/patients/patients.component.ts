import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { FormValidateDirective } from 'form-validate-angular';
import { PatientPipe } from '../../pipe/patient.pipe';
import { RouterLink } from '@angular/router';
import { PatientModel } from '../../models/patient.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';

@Component({
  selector: 'app-patient',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    FormValidateDirective,
    PatientPipe,
  ],
  templateUrl: './patients.component.html',
  styleUrl: './patients.component.css',
})
export class PatientsComponent implements OnInit {
  patients: PatientModel[] = [];

  search: string = '';

  @ViewChild('addModalCloseBtn') addModalCloseBtn:
    | ElementRef<HTMLButtonElement>
    | undefined;

  @ViewChild('updateModalCloseBtn') updateModalCloseBtn:
    | ElementRef<HTMLButtonElement>
    | undefined;

  createModel: PatientModel = new PatientModel();
  updateModel: PatientModel = new PatientModel();

  constructor(private http: HttpService, private swal: SwalService) {}

  ngOnInit(): void {
    this.getAll();
  }

  getAll() {
    this.http.post<PatientModel[]>('Patients/GetAll', {}, (res) => {
      this.patients = res.data;
    });
  }

  add(form: NgForm) {
    if (form.valid) {
      this.http.post<string>('Patients/Create', this.createModel, (res) => {
        this.swal.callToast(res.data, 'success');
        this.getAll();
        this.addModalCloseBtn?.nativeElement.click();
        this.createModel = new PatientModel();
      });
    }
  }

  delete(id: string, fullName: string) {
    this.swal.callSwal(
      'Delete patient?',
      `You want to delete ${fullName}?`,
      'Delete',
      () => {
        this.http.post<string>('Patients/DeleteById', { id: id }, (res) => {
          this.swal.callToast(res.data, 'info');
          this.getAll();
        });
      }
    );
  }

  update(fullName: string, form: NgForm) {
    if (form.valid) {
      this.swal.callSwal(
        'Update patient?',
        `You want to update ${fullName}`,
        'Update',
        () => {
          this.http.post<string>('Patients/Update', this.updateModel, (res) => {
            this.swal.callToast(res.data, 'success');
            this.getAll();
            this.updateModalCloseBtn?.nativeElement.click();
            this.updateModel = new PatientModel();
          });
        }
      );
    }
  }

  fillUpdateModal(patient: PatientModel) {
    this.updateModel = { ...patient };
  }
}
