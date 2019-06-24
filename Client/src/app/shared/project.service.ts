import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {


  constructor(private fb: FormBuilder, private http: HttpClient) { }

  readonly BaseURI = 'http://localhost:53057/api';

  formModel = this.fb.group({
    ProjectName: ['', Validators.required],
    Description: ['', Validators.required],
    ImageURL: ['']
  });

  registerProject() {
    const body = {
        ProjectName: this.formModel.value.ProjectName,
        Description: this.formModel.value.Description,
        ImageURL: this.formModel.value.ImageURL
    };
    return this.http.post(this.BaseURI + '/Project/AddProject', body);
  }

  removeProject(projectId: number) {
    const body = {
        Id: projectId
    };
    return this.http.post(this.BaseURI + '/Project/RemoveProject', body);
  }

  getAllProjects() {
    return this.http.get(this.BaseURI + '/Project/GetAllProjects');
  }

  changeDone(projectId: number) {
    const body = {
        Id: projectId
    };
    return this.http.post(this.BaseURI + '/Project/ChangeDone', body);
}

}
