import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { CinemasComponent } from './cinemas/cinemas.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { UserTicketsComponent } from './user-tickets/user-tickets.component';

const routes: Routes = [
  { path: '', component: CinemasComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'tickets', component: UserTicketsComponent, children: [] },
  { path: '**', pathMatch: 'full', component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
