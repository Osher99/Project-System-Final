import { Component, OnInit } from '@angular/core';
import { UserService } from '../shared/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-privatepanel',
  templateUrl: './privatepanel.component.html',
  styles: []
})
export class PrivatepanelComponent implements OnInit {
  constructor(private service: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.service.formPasswordModel.reset();
  }

  onSubmit() {
    this.service.changePassword().subscribe(
      (result: any) => { if (result) { 
          this.service.formPasswordModel.reset();
          this.toastr.success('Password changed succesfully!', 'Success');
      }
      },
      err => {
        this.toastr.error('Failed to change password', err);
      }
    );
  }
}
