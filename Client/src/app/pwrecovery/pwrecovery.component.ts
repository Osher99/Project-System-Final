import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserService } from '../shared/user.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-pwrecovery',
  templateUrl: './pwrecovery.component.html',
  styles: []
})
export class PwrecoveryComponent implements OnInit {
  formModel = {
    Email: ''
  };

  constructor(private userService: UserService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
  }
  onSubmit(form: NgForm) {
    this.userService.recoverPassword(form.value).subscribe(
      (res: any) => {
        this.toastr.success('Email sent to ' + form.value.Email);
      },
      (err: any) => {
        if (err.status === 400) {
          this.toastr.error('No user with such Email found!', 'Revcovery failed');
          form.reset();
        } else {
          this.toastr.error('Our servers is down at the moment', 'Try again later');
          form.reset();
        }
      }
    );
  }

}
