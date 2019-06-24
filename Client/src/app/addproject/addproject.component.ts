import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ProjectService } from '../shared/project.service';

@Component({
  selector: 'app-addproject',
  templateUrl: './addproject.component.html',
  styles: []
})
export class AddprojectComponent implements OnInit {

  constructor(private service: ProjectService, private toastr: ToastrService) { }

  ngOnInit() {
  }

  onSubmit() {
    this.service.registerProject().subscribe(
      (result: boolean) => {
        if (result) {
        this.toastr.success('New porject created!', 'Project Creation Process Successful');
        this.service.formModel.reset();
        } else {
                  this.toastr.error('Project Creation Process failed!', 'Validation Error');
            }
          },
      err => {
        this.toastr.error('Project Creation Process failed!', 'Server Error');
      });
    }
  }
