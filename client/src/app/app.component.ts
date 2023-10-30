import { Component } from '@angular/core';
import { Router } from '@angular/router';

type Type1 = {
  age: number
  name: string
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'client';
}
