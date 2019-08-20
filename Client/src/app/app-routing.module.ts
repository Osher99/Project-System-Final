import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegisterComponent } from './user/register/register.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './auth/auth.guard';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { EmailconfirmedComponent } from './emailconfirmed/emailconfirmed.component';
import { AddprojectComponent } from './addproject/addproject.component';
import { MyprojectsComponent } from './myprojects/myprojects.component';
import { PwrecoveryComponent } from './pwrecovery/pwrecovery.component';
import { PrivatepanelComponent } from './privatepanel/privatepanel.component';
import { PaymentDetailsComponent } from './payment-details/payment-details.component';

const routes: Routes = [
  { path: '', redirectTo: '/user/login', pathMatch: 'full' },
  { path: 'user', component: UserComponent,
children: [
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'pwrecovery', component: PwrecoveryComponent }
]
},
{ path: 'home', component: HomeComponent, canActivate: [AuthGuard], children: [
  { path: 'paymentpage', component: PaymentDetailsComponent },
  { path: 'privatepanel', component: PrivatepanelComponent },
  { path: 'newproject', component: AddprojectComponent },
  { path: 'myprojects', component: MyprojectsComponent },
  { path: 'forbidden', component: ForbiddenComponent },
  { path: 'adminpanel', component: AdminPanelComponent, canActivate: [AuthGuard], data: {permittedRoles: ['admin']} }
] },
{path: 'emailsuccess', component: EmailconfirmedComponent},
{path: '404', component: NotfoundComponent},
{path: '**', redirectTo: '/404'}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
