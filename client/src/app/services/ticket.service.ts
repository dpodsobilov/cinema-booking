import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Tickets {
  upcomingTickets: Ticket[];
  pastTickets: Ticket[];
}

export interface Ticket {
  ticketId: number;
  date: string;
  filmName: string;
  cinemaName: string;
  cinemaHallName: string;
  placeName: string;
  placeTypeName: string;
}

@Injectable({
  providedIn: 'root',
})
export class TicketService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getUserTickets(userId: number): Observable<Tickets> {
    return this.http.post<Tickets>(this.baseUrl + '/Ticket', userId);
  }
}
