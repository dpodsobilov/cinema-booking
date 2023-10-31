import { Component } from '@angular/core';

@Component({
  selector: 'app-user-tickets',
  templateUrl: './user-tickets.component.html',
  styleUrls: ['./user-tickets.component.css'],
})
export class UserTicketsComponent {
  isUpcoming: boolean = true;

  // oncinemaSelected(id: number){
  //   let counter = 1
  //
  //   for(let i = 0; i<this.temp.length; i++){
  //     if(this.temp.includes(id)){
  //       counter++
  //     }
  //     else{this.temp = []}
  //   }
  //
  //   this.temp.push(id)
  //
  //   if (counter%2!==0){
  //     for(let i = 0; i < this.cinemaFilms.length; i++ ){
  //       if(id === this.cinemaFilms[i].cinemaId){
  //         this.films = this.cinemaFilms[i].films
  //       }
  //       (document.getElementById(String(this.cinemaFilms[i].cinemaId)) as HTMLElement).style.backgroundColor = "transparent"
  //     }
  //     (document.getElementById(String(id)) as HTMLElement).style.backgroundColor = "#1DE782"
  //   }
  //   else{
  //     (document.getElementById(String(id)) as HTMLElement).style.backgroundColor = "transparent"
  //     this.addAllFilms()
  //   }
  // }

  showTickets() {
    this.isUpcoming = !this.isUpcoming;
  }
}
