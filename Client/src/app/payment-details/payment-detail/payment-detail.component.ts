import { Component, OnInit } from '@angular/core';
import { PaymentDetailService } from 'src/app/shared/payment-detail.service';
import { PaymentDetail } from 'src/app/models/payment-detail';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-payment-detail',
  templateUrl: './payment-detail.component.html',
  styles: []
})
export class PaymentDetailComponent implements OnInit {

  constructor(private paymentService: PaymentDetailService,
              private toastrService: ToastrService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form?: NgForm) {

    if (form != null) {
    form.resetForm();
    }

    this.paymentService.formData = {
      PMId: 0,
      CardNumber: '',
      CardOwnerName: '',
      CVV: '',
      ExpirationDate: ''
  };
}

onSubmit(form: NgForm) {
  if (this.paymentService.formData.PMId === 0) {
    this.insertRecord(form);
  } else {
    this.updateRecord(form);
  }
}

insertRecord(form: NgForm) {
  this.paymentService.postPaymentDetail().subscribe(
    res => {
      this.toastrService.success('Card Submitted Successfully', 'Payment Detail Register');
      this.paymentService.refreshList();
      this.resetForm(form);
    },
    err => {
      console.log(err);
    }
  );
}

updateRecord(form: NgForm) {
  this.paymentService.putPaymentDetail().subscribe(
    res => {
      this.resetForm(form);
      this.toastrService.info('Card Updated Successfully', 'Payment Detail Update');
      this.paymentService.refreshList();
    },
    err => {
      console.log(err);
    }
  );
}

}
