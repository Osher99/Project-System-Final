import { Component, OnInit } from '@angular/core';
import { PaymentDetailService } from 'src/app/shared/payment-detail.service';
import { PaymentDetail } from 'src/app/models/payment-detail';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-payment-detail-list',
  templateUrl: './payment-detail-list.component.html',
  styles: []
})
export class PaymentDetailListComponent implements OnInit {

  constructor(private paymentService: PaymentDetailService,
              private toastrService: ToastrService) { }

  ngOnInit() {
    this.paymentService.refreshList();
  }

  populateForm(pd: PaymentDetail) {
    this.paymentService.formData = Object.assign({}, pd);
  }

  onDelete(PMId: number) {
    if (confirm('Are you sure to delete this record? ')) {
    this.paymentService.deletePaymentDetail(PMId)
    .subscribe(
      res => {
        this.paymentService.refreshList();
        this.toastrService.warning('Successfully deleted the record', 'Payment Detail delete');
      },
      err => {
        console.log(err);
      }
    );
  }
}
}
