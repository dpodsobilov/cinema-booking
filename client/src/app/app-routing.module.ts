import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './user/register/register.component';
import { LoginComponent } from './user/login/login.component';
import { CinemasComponent } from './user/cinemas/cinemas.component';
import { NotFoundComponent } from './user/not-found/not-found.component';
import { UserTicketsComponent } from './user/user-tickets/user-tickets.component';
import { FilmComponent } from './user/film/film.component';
import { PlacesComponent } from './user/places/places.component';
import { OrderComponent } from './user/order/order.component';
import { SuccessComponent } from './user/success/success.component';
import { ErrorComponent } from './user/error/error.component';
import { MainLayoutComponent } from './shared/components/main-layout/main-layout.component';
import { adminGuard } from './services/admin/admin.guard';
import { loginGuard } from './services/login.guard';
import { SystemInfoComponent } from './user/system-info/system-info.component';
import { InstructionComponent } from './user/system-info/instruction/instruction.component';
import { DevInfoComponent } from './user/system-info/dev-info/dev-info.component';

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: '', redirectTo: '/', pathMatch: 'full' },
      { path: '', component: CinemasComponent },
      {
        path: 'register',
        canActivate: [loginGuard],
        component: RegisterComponent,
      },
      { path: 'login', canActivate: [loginGuard], component: LoginComponent },
      { path: 'film/:id', component: FilmComponent },
      { path: 'places', component: PlacesComponent },
      { path: 'order', component: OrderComponent },
      { path: 'order-status', component: SuccessComponent },
      { path: 'error', component: ErrorComponent },
      { path: 'tickets', component: UserTicketsComponent },
      {
        path: 'system-info',
        component: SystemInfoComponent,
        children: [
          {
            path: 'instruction',
            component: InstructionComponent,
          },
          {
            path: 'dev-info',
            component: DevInfoComponent,
          },
        ],
      },
    ],
  },
  {
    path: 'admin',
    canActivate: [adminGuard],
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
