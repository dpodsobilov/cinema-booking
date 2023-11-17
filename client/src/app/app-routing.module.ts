import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { CinemasComponent } from './cinemas/cinemas.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { UserTicketsComponent } from './user-tickets/user-tickets.component';
import { FilmComponent } from './film/film.component';
import { PlacesComponent } from './places/places.component';
import { OrderComponent } from './order/order.component';
import { SuccessComponent } from './success/success.component';
import { ErrorComponent } from './error/error.component';
import { MainLayoutComponent } from './shared/components/main-layout/main-layout.component';

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: '', redirectTo: '/', pathMatch: 'full' },
      { path: '', component: CinemasComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'login', component: LoginComponent },
      { path: 'film/:id', component: FilmComponent },
      { path: 'places', component: PlacesComponent },
      { path: 'order', component: OrderComponent },
      { path: 'order-status', component: SuccessComponent },
      { path: 'error', component: ErrorComponent },
      { path: 'tickets', component: UserTicketsComponent },
    ],
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('./admin/admin.module').then((m) => m.AdminModule),
  },
  { path: '**', pathMatch: 'full', component: NotFoundComponent },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      preloadingStrategy: PreloadAllModules,
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
