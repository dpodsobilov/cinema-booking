import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { CinemasComponent } from './cinemas/cinemas.component';
import { FilmComponent } from './film/film.component';
import { PlacesComponent } from './places/places.component';

const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: '', component: CinemasComponent },
  { path: 'film/:id', component: FilmComponent },
  { path: 'places', component: PlacesComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
