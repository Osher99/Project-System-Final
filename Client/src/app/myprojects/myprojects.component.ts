import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../shared/project.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-myprojects',
  templateUrl: './myprojects.component.html',
  styles: []
})
export class MyprojectsComponent implements OnInit {
  projectList: any;

  constructor(private projectService: ProjectService, private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    this.projectService.getAllProjects().subscribe(
      (res: any) => {
       this.projectList = res;
      },
      err => {
        console.log(err);
      }
    );
}

onRemove(projectId: number) {
  this.projectService.removeProject(projectId).subscribe(
    (res: boolean) => {
      if (res) {
        this.toastr.success('Project removed successfully', 'Success');
        this.ngOnInit();
          }
    },
      err => {
        this.toastr.error('Please try again later', err);
        console.log(err);
      });
}

onChangeDone(projectId: number) {
  this.projectService.changeDone(projectId).subscribe(
    (res: boolean) => {
      if (res) {
        this.toastr.success('Project updated successfully', 'Updated');
        this.ngOnInit();
        }
    },
      err => {
        this.toastr.error('Please try again later', err);
        console.log(err);
      });
}
}
