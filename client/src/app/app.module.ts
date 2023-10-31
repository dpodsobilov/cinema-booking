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
import { NotFoundComponent } from './not-found/not-found.component';
import { UserTicketsComponent } from './user-tickets/user-tickets.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    NavbarComponent,
    CinemasComponent,
    NotFoundComponent,
    UserTicketsComponent,
  ],
  imports: [
    FormsModule,
    AppRoutingModule,
    BrowserModule,
    ReactiveFormsModule,
    RouterOutlet,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
