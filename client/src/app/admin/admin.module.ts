import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AdminLayoutComponent } from './shared/components/admin-layout/admin-layout.component';
import { FilmsComponent } from './films/films.component';
import { AdminCinemasComponent } from './admin-cinemas/admin-cinemas.component';
import { TemplatesComponent } from './templates/templates.component';
import { SessionsComponent } from './sessions/sessions.component';
import { UsersComponent } from './users/users.component';
import { StatsComponent } from './stats/stats.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    AdminLayoutComponent,
    FilmsComponent,
    AdminCinemasComponent,
    TemplatesComponent,
    SessionsComponent,
    UsersComponent,
    StatsComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      {
        path: '',
        component: AdminLayoutComponent,
        children: [
          { path: '', redirectTo: './login', pathMatch: 'full' },
          { path: 'films', component: FilmsComponent },
          { path: 'cinemas', component: AdminCinemasComponent },
          { path: 'templates', component: TemplatesComponent },
          { path: 'sessions', component: SessionsComponent },
          { path: 'users', component: UsersComponent },
          { path: 'stats', component: StatsComponent },
        ],
      },
    ]),
  ],
  exports: [RouterModule],
})
export class AdminModule {}
