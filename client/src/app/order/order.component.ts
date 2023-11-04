import { Component } from '@angular/core';

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
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css'],
})
export class OrderComponent {
  ticket: Ticket = {
    date: 'Чт, 10 октября 2023',
    filmName: 'Человек-Паук',
    cinemaName: 'Кинотеатр 2',
    cinemaHallName: 'Зал 1 - VIP',
    seatsQuantity: 3,
    seats: [2, 3, 4],
    time: new Date().toLocaleTimeString().slice(0, 5),
  };
}
