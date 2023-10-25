import { Component } from '@angular/core';

@Component({
  selector: 'app-film',
  templateUrl: './film.component.html',
  styleUrls: ['./film.component.css']
})
export class FilmComponent {

  selectcinemahall = document.getElementById('selectcinemahall')
  cinemas:string[] = []
  halls:string[] = []
  temp:string[]=[]
  CINEMA = [
    {
      cinema: "Кинотеатр 1",
      hall: ["Стандарт", "Комфорт", "VIP"],
    },
    {
      cinema: "Кинотеатр 2",
      hall: ["Стандарт", "Комфорт"],
    }
  ]
  constructor() {
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
          this.halls = this.CINEMA[i].hall
        }
        (document.getElementById(this.CINEMA[i].cinema) as HTMLElement).style.backgroundColor = "transparent"
      }
      (document.getElementById(value) as HTMLElement).style.backgroundColor = "#1DE782"
    }
    else{
      (document.getElementById(value) as HTMLElement).style.backgroundColor = "transparent"
      this.halls = []
    }
  }
  onhallSelected(value: string){
    if(value !== 'default'){

    }
  }
}
