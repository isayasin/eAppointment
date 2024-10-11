import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class SwalService {
  constructor() {}

  callToast(title: string, icon: SweetAlertIcon = 'success') {
    Swal.fire({
      title: title,
      position: 'bottom-right',
      timer: 3000,
      icon: icon,
      showCancelButton: false,
      showConfirmButton: false,
      showCloseButton: true,
      toast: true,
    });
  }

  callSwal(
    title: string,
    text: string,
    confirmBtnName: string ,
    callBack: () => void
  ) {
    Swal.fire({
      title: title,
      text: text,
      icon: 'question',
      showCancelButton: true,
      cancelButtonText: 'Cancel',
      showConfirmButton: true,
      confirmButtonText: confirmBtnName,
    }).then((res) => {
      if (res.isConfirmed) {
        callBack();
      }
    });
  }
}

export type SweetAlertIcon =
  | 'success'
  | 'error'
  | 'warning'
  | 'info'
  | 'question';
