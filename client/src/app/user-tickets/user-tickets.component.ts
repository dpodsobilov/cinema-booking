import { Component } from '@angular/core';
import { tick } from '@angular/core/testing';

export interface Ticket {
  date: string;
  filmName: string;
  cinemaName: string;
  cinemaHallName: string;
  seatsQuantity: number;
  seats: number[];
  time: string;
}

@Component({
  selector: 'app-user-tickets',
  templateUrl: './user-tickets.component.html',
  styleUrls: ['./user-tickets.component.css'],
})
export class UserTicketsComponent {
  isUpcoming: boolean = true;

  upcomingTickets: Ticket[] = [
    {
      date: 'Пн, 23 октября 2023',
      filmName: 'Человек-Паук',
      cinemaName: 'Кинотеатр 2',
      cinemaHallName: 'Зал 2 - Стандарт',
      seatsQuantity: 3,
      seats: [2, 3, 4],
      time: new Date().toLocaleTimeString().slice(0, 5),
    },
    {
      date: 'Ср, 25 октября 2023',
      filmName: 'Человек-Паук',
      cinemaName: 'Кинотеатр 2',
      cinemaHallName: 'Зал 2 - Стандарт',
      seatsQuantity: 3,
      seats: [2, 3, 4],
      time: new Date().toLocaleTimeString().slice(0, 5),
    },
    {
      date: 'Чт, 23 октября 2023',
      filmName: 'Человек-Паук',
      cinemaName: 'Кинотеатр 2',
      cinemaHallName: 'Зал 2 - Стандарт',
      seatsQuantity: 3,
      seats: [2, 3, 4],
      time: new Date().toLocaleTimeString().slice(0, 5),
    },
    {
      date: 'Чт, 23 октября 2023',
      filmName: 'Человек-Паук',
      cinemaName: 'Кинотеатр 2',
      cinemaHallName: 'Зал 2 - Стандарт',
      seatsQuantity: 3,
      seats: [2, 3, 4],
      time: new Date().toLocaleTimeString().slice(0, 5),
    },
  ];

  pastTickets: Ticket[] = [
    {
      date: 'Чт, 10 октября 2023',
      filmName: 'Человек-Паук',
      cinemaName: 'Кинотеатр 2',
      cinemaHallName: 'Зал 1 - VIP',
      seatsQuantity: 3,
      seats: [2, 3, 4],
      time: new Date().toLocaleTimeString().slice(0, 5),
    },
  ];
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
}
