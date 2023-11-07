import { Injectable } from '@angular/core';

export interface Ticket {
  id: number;
  date: string;
  filmName: string;
  cinemaName: string;
  cinemaHallName: string;
  seatsQuantity: number;
  seats: number[];
  time: string;
}

@Injectable({
  providedIn: 'root',
})
export class TicketService {
  constructor() {}
}
