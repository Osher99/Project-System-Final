import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/shared/user.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {
  formModel = {
    UserName: '',
    Password: ''
  };
  constructor(private userService: UserService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
    if (localStorage.getItem('token') != null) {
      this.router.navigateByUrl('/home');
    }
  }

  onSubmit(form: NgForm, userName: string) {
    this.userService.login(form.value).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
        this.router.navigateByUrl('/home');
        this.toastr.success('Welcome ' + userName + '!' , 'Authentication granted');
      },
      (err: any) => {
        if (err.status === 400) {
          this.toastr.error('Incorrect User name of Password!', 'Authentication failed');
          form.reset();
        } else {
          this.toastr.error('Our servers is down at the moment', 'Try again later');
          form.reset();
        }
      }
    );
  }
}
