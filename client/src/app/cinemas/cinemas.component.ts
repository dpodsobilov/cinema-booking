import {Component, Inject} from '@angular/core';
export interface Film{
  name: string;
  // poster: string;
}

const film1: string = "Пираты карибского моря"
const film2: string = "Железный человек"
const film3: string = "Человек паук"
const film4: string = "Зеленая миля"
const film5: string = "Аватар"
const film6: string = "Криминальное чтиво"

@Component({
    selector: 'app-cinemas',
    templateUrl: './cinemas.component.html',
    styleUrls: ['./cinemas.component.css']
})
export class CinemasComponent {
  films:string[] = []
  cinemas:string[] = []
  temp:string[]=[]
  CINEMA = [
      {
          cinema: "Кинотеатр 1",
          films: [film1, film2],
      },
      {
          cinema: "Кинотеатр 2",
          films: [film3, film4, film5, film6],
      }
  ]
  constructor() {
      this.films = [film1, film2, film3, film4, film5, film6]
      for(let i = 0; i < this.CINEMA.length; i++ ){
          this.cinemas[i] = this.CINEMA[i].cinema
      }
  }

  oncinemaSelected(value: string){
    let counter = 1

    for(let i = 0; i<this.temp.length; i++){
      if(this.temp.includes(value)){
        counter++
      }
      else{this.temp = []}
    }

    this.temp.push(value)

    if (counter%2!==0){
      for(let i = 0; i < this.CINEMA.length; i++ ){
        if(value === this.CINEMA[i].cinema){
          this.films = this.CINEMA[i].films
        }
        (document.getElementById(this.CINEMA[i].cinema) as HTMLElement).style.backgroundColor = "transparent"
      }
      (document.getElementById(value) as HTMLElement).style.backgroundColor = "#1DE782"
    }
    else{
      (document.getElementById(value) as HTMLElement).style.backgroundColor = "transparent"
      this.films = [film1, film2, film3, film4, film5, film6]
    }
  }
}



