import { Component } from '@angular/core';

export interface Film{
  name: string;
  // poster: string;
}
@Component({
  selector: 'app-cinemas',
  templateUrl: './cinemas.component.html',
  styleUrls: ['./cinemas.component.css']
})
export class CinemasComponent {

  film1 = "Пираты карибского моря"
  film2= "Железный человек"
  film3= "Человек паук"
  film4= "Зеленая миля"
  film5= "Аватар"
  film6= "Криминальное чтиво"

  films: string[] = [this.film1, this.film2, this.film3,this.film4,this.film5,this.film6]

}
