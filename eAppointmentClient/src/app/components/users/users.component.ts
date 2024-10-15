import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { FormValidateDirective } from 'form-validate-angular';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { UserPipe } from '../../pipe/user.pipe';
import { UserModel } from '../../models/user.model';
import { RoleModel } from '../../models/role.model';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    FormValidateDirective,
    UserPipe,
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
})
export class UsersComponent implements OnInit {
  users: UserModel[] = [];
  roles: RoleModel[] = [];

  search: string = '';

  @ViewChild('addModalCloseBtn') addModalCloseBtn:
    | ElementRef<HTMLButtonElement>
    | undefined;

  @ViewChild('updateModalCloseBtn') updateModalCloseBtn:
    | ElementRef<HTMLButtonElement>
    | undefined;

  createModel: UserModel = new UserModel();
  updateModel: UserModel = new UserModel();

  constructor(private http: HttpService, private swal: SwalService) {}

  ngOnInit(): void {
    this.getAll();
    this.getAllRoles();
  }

  getAll() {
    this.http.post<UserModel[]>('Users/GetAll', {}, (res) => {
      this.users = res.data;
    });
  }

  getAllRoles() {
    this.http.post<RoleModel[]>('Users/GetAllRoles', {}, (res) => {
      this.roles = res.data;
    });
  }

  add(form: NgForm) {
    if (form.valid) {
      this.http.post<string>('Users/Create', this.createModel, (res) => {
        this.swal.callToast(res.data, 'success');
        this.getAll();
        this.addModalCloseBtn?.nativeElement.click();
        this.createModel = new UserModel();
      });
    }
  }

  delete(id: string, fullName: string) {
    this.swal.callSwal(
      'Delete users?',
      `You want to delete ${fullName}?`,
      'Delete',
      () => {
        this.http.post<string>('Users/DeleteById', { id: id }, (res) => {
          this.swal.callToast(res.data, 'info');
          this.getAll();
        });
      }
    );
  }

  update(fullName: string, form: NgForm) {
    if (form.valid) {
      this.swal.callSwal(
        'Update user?',
        `You want to update ${fullName}`,
        'Update',
        () => {
          this.http.post<string>('Users/Update', this.updateModel, (res) => {
            this.swal.callToast(res.data, 'success');
            this.getAll();
            this.updateModalCloseBtn?.nativeElement.click();
            this.updateModel = new UserModel();
          });
        }
      );
    }
  }

  fillUpdateModal(user: UserModel) {
    this.updateModel = { ...user };
    
  }
}
