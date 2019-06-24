import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/shared/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styles: []
})
export class RegisterComponent implements OnInit {
  //
  constructor(private service: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.service.formModel.reset();
  }

  onSubmit() {
    this.service.register().subscribe(
      (result: any) => {
        if (result.succeeded) {
          this.service.formModel.reset();
          this.toastr.success('New user created!', 'Register Process Successful');
        } else {
          result.errors.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
                  this.toastr.error('User Name is already taken!', 'Register Process failed');
                  break;
                default:
                    this.toastr.error(element.description, 'Register Process failed');
                    break;
            }
          });
        }
      },
      err => {
        this.toastr.error(err.description, 'Register Process failed, try to change email');
      }
    );
  }

}
