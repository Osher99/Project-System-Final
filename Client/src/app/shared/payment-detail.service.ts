import { Injectable } from '@angular/core';
import { PaymentDetail } from '../models/payment-detail';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PaymentDetailService {
  formData: PaymentDetail;
  private readonly rootURL = 'http://localhost:53057/api';
  cardList: PaymentDetail[];

  constructor(private http: HttpClient) {}

  postPaymentDetail() {
    return this.http.post(this.rootURL + '/PaymentDetail', this.formData);
  }

  putPaymentDetail() {
    return this.http.put(this.rootURL + '/PaymentDetail/' + this.formData.PMId, this.formData);
  }

  deletePaymentDetail(id: number) {
    return this.http.delete(this.rootURL + '/PaymentDetail/' + id);
  }

  refreshList() {
    this.http.get(this.rootURL + '/PaymentDetail')
    .toPromise()
    .then(res => this.cardList = res as PaymentDetail[])
    .catch((err) =>
    console.log(err));
  }

}
