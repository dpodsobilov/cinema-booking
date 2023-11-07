import { Component } from '@angular/core';
import { Ticket } from '../services/ticket.service';

@Component({
  selector: 'app-user-tickets',
  templateUrl: './user-tickets.component.html',
  styleUrls: ['./user-tickets.component.css'],
})
export class UserTicketsComponent {
  isUpcoming: boolean = true;

  tickets: Ticket[] = [];

  upcomingTickets: Ticket[] = [
    {
      id: 1,
      date: 'Пн, 23 октября 2023',
      filmName: 'Человек-Паук',
      cinemaName: 'Кинотеатр 2',
      cinemaHallName: 'Зал 2 - Стандарт',
      seatsQuantity: 3,
      seats: [2, 3, 4],
      time: new Date().toLocaleTimeString().slice(0, 5),
    },
    {
      id: 2,
      date: 'Ср, 25 октября 2023',
      filmName: 'Человек-Паук',
      cinemaName: 'Кинотеатр 2',
      cinemaHallName: 'Зал 2 - Стандарт',
      seatsQuantity: 3,
      seats: [2, 3, 4],
      time: new Date().toLocaleTimeString().slice(0, 5),
    },
    {
      id: 2,
      date: 'Чт, 23 октября 2023',
      filmName: 'Человек-Паук',
      cinemaName: 'Кинотеатр 2',
      cinemaHallName: 'Зал 2 - Стандарт',
      seatsQuantity: 3,
      seats: [2, 3, 4],
      time: new Date().toLocaleTimeString().slice(0, 5),
    },
    {
      id: 3,
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
      id: 4,
      date: 'Чт, 10 октября 2023',
      filmName: 'Человек-Паук',
      cinemaName: 'Кинотеатр 2',
      cinemaHallName: 'Зал 1 - VIP',
      seatsQuantity: 3,
      seats: [2, 3, 4],
      time: new Date().toLocaleTimeString().slice(0, 5),
    },
  ];

  constructor() {
    this.tickets = this.upcomingTickets;
  }
}
