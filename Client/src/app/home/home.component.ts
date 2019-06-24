import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: []
})
export class HomeComponent implements OnInit {
  userDetails: object;
  verified: boolean;
  admin: boolean;

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit() {
    this.userService.getUserProfile().subscribe(
      (res: object) => {
        this.userDetails = res;
      },
      err => {
        console.log(err);
      }
    );

    this.userService.ifVerified().subscribe(
      (flag: boolean) => {
        this.verified = flag;
      },
      err => {
        console.log(err);
      }
    );

    this.userService.isAdmin().subscribe(
      (flag: boolean) => {
        this.admin = flag;
      },
      err => {
        console.log(err);
      }
    );
  }

  onLogout() {
   localStorage.removeItem('token');
   this.router.navigate(['/user/login']);
  }
}
