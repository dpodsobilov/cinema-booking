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
import { AdminPaneComponent } from './admin-pane/admin-pane.component';
import { GenresComponent } from './genres/genres.component';
import { HallsComponent } from './halls/halls.component';
import { NotFoundComponent } from '../user/not-found/not-found.component';
import { AddTemplateComponent } from './add-template/add-template.component';
import { PlaceTypeComponent } from './place-type/place-type.component';
import { GenreModalComponent } from './modals/genre-modal/genre-modal.component';
import { PlaceTypeModalComponent } from './modals/place-type-modal/place-type-modal.component';

@NgModule({
  declarations: [
    AdminLayoutComponent,
    FilmsComponent,
    AdminCinemasComponent,
    TemplatesComponent,
    SessionsComponent,
    UsersComponent,
    StatsComponent,
    AdminPaneComponent,
    GenresComponent,
    HallsComponent,
    AddTemplateComponent,
    PlaceTypeComponent,
    GenreModalComponent,
    PlaceTypeModalComponent,
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
          { path: 'templates/add-template', component: AddTemplateComponent },
        ],
      },
    ]),
  ],
  exports: [RouterModule],
})
export class AdminModule {}
