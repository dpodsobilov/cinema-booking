import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { AppRoutingModule } from './app-routing.module';
import { CinemasComponent } from './user/cinemas/cinemas.component';
import { NotFoundComponent } from './user/not-found/not-found.component';
import { UserTicketsComponent } from './user/user-tickets/user-tickets.component';
import { FilmComponent } from './user/film/film.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AuthInterceptor } from './services/auth.interceptor';
import { PlacesComponent } from './user/places/places.component';
import { OrderComponent } from './user/order/order.component';
import { SuccessComponent } from './user/success/success.component';
import { ErrorComponent } from './user/error/error.component';
import { TicketComponent } from './user/ticket/ticket.component';
import { MainLayoutComponent } from './shared/components/main-layout/main-layout.component';
import { SharedModule } from './shared/shared.module';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    NotFoundComponent,
    UserTicketsComponent,
    CinemasComponent,
    FilmComponent,
    PlacesComponent,
    OrderComponent,
    SuccessComponent,
    ErrorComponent,
    TicketComponent,
    MainLayoutComponent,
  ],
  imports: [AppRoutingModule, BrowserModule, SharedModule],
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
