import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { NavbarComponent } from './navbar/navbar.component';
import { CinemasComponent } from './cinemas/cinemas.component';
import { FilmComponent } from './film/film.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AuthInterceptor } from './services/auth.interceptor';
import { PlacesComponent } from './places/places.component';
import { OrderComponent } from './order/order.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    NavbarComponent,
    CinemasComponent,
    FilmComponent,
    PlacesComponent,
    OrderComponent,
  ],
  imports: [
    FormsModule,
    AppRoutingModule,
    BrowserModule,
    ReactiveFormsModule,
    RouterOutlet,
    HttpClientModule,
  ],
  providers: [
    {
      provide: 'BASE_API_URL',
      useValue: 'http://localhost:5000',
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
