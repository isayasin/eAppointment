import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ResultModel } from '../models/result.model';
import { api } from '../constant';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  token: string = '';

  constructor(private http: HttpClient, private error: ErrorService) {
    this.token = localStorage.getItem('token') ?? '';
  }

  post<T>(
    apiUrl: string,
    body: any,
    callback: (res: ResultModel<T>) => void,
    errCallBack?: (err: HttpErrorResponse) => void
  ) {
    this.http
      .post<ResultModel<T>>(`${api}/${apiUrl}`, body, {
        headers: {
          Authorization: 'Bearer ' + this.token,
        },
      })
      .subscribe({
        next: (res) => {
          callback(res);
        },
        error: (err: HttpErrorResponse) => {
          this.error.errorHandler(err);
          if (errCallBack != undefined) {
            errCallBack(err);
          }
        },
      });
  }
}
