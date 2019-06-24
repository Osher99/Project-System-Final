import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {


  constructor(private fb: FormBuilder, private http: HttpClient) { }

  readonly BaseURI = 'http://localhost:53057/api';

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', [Validators.required, Validators.email]],
    FullName: [''],
    Passwords: this.fb.group({
      Password: ['', [Validators.required, Validators.minLength(4)]],
      ConfirmPassword: ['', Validators.required],
    }, {validator: this.comparePasswords }),
    SpecialCode: ['']
  });

  formPasswordModel = this.fb.group({
    CurrentPassword: ['', [Validators.required, Validators.minLength(4)]],
    Passwords: this.fb.group({
      Password: ['', [Validators.required, Validators.minLength(4)]],
      ConfirmPassword: ['', Validators.required],
    }, {validator: this.comparePasswords }),
  });


  ifVerified() {
    return this.http.get(this.BaseURI + '/UserProfile/IsVerified');
  }

  isAdmin() {
    return this.http.get(this.BaseURI + '/UserProfile/isAdmin');
  }

  recoverPassword(formData: any) {
    return this.http.post(this.BaseURI + '/AppUser/RecoverPassword', formData);
  }

  comparePasswords(fb: FormGroup) {
    const confirmPasswordsCtrl = fb.get('ConfirmPassword');

    if (confirmPasswordsCtrl.errors == null || 'passwordMismatch' in confirmPasswordsCtrl.errors) {
      if (fb.get('Password').value !== confirmPasswordsCtrl.value) {
        confirmPasswordsCtrl.setErrors({passwordMismatch: true});
      } else {
        confirmPasswordsCtrl.setErrors(null);
      }
    }
  }
  changePassword() {

    const body = {
      CurrentPassword: this.formPasswordModel.value.CurrentPassword,
      Password: this.formPasswordModel.value.Passwords.Password,
      ConfirmPassword: this.formPasswordModel.value.Passwords.ConfirmPassword
    };

    return this.http.post(this.BaseURI + '/AppUser/ChangePassword', body);

  }
  register() {
    const body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      FullName: this.formModel.value.FullName,
      Password: this.formModel.value.Passwords.Password,
      SpecialCode: this.formModel.value.SpecialCode
    };
    return this.http.post(this.BaseURI + '/AppUser/Register', body);
  }

  login(formData: any) {
    return this.http.post(this.BaseURI + '/AppUser/Login', formData);
  }

  getUserProfile() {
    // const tokenHeader = new HttpHeaders({
     // Authorization: 'Bearer ' + localStorage.getItem('token')
    // });
    return this.http.get(this.BaseURI + '/UserProfile');
  }

  roleMatch(allowedRoles: string[]): boolean {
    let isMatch = false;
    const payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    const userRole = payLoad.role;

    allowedRoles.forEach((element: any) => {
      if (userRole === element) {
        isMatch = true;
        return false;
      }
    });
    return isMatch;
  }
}
