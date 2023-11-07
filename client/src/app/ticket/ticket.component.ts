import { Component, Input } from '@angular/core';
import { Ticket } from '../services/ticket.service';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css'],
})
export class TicketComponent {
  @Input('ticket') ticket: Ticket = {
    id: 0,
    date: '',
    time: '',
    seats: [],
    seatsQuantity: 0,
    cinemaHallName: '',
    cinemaName: '',
    filmName: '',
  };

  @Input('isUpcoming') isUpcoming = false;
}
