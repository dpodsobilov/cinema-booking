import { Component, OnInit } from '@angular/core';
import { Ticket, Tickets, TicketService } from '../services/ticket.service';
import { UserInfo } from '../services/auth.service';

@Component({
  selector: 'app-user-tickets',
  templateUrl: './user-tickets.component.html',
  styleUrls: ['./user-tickets.component.css'],
})
export class UserTicketsComponent implements OnInit {
  isUpcoming: boolean = true;
  userId: number = 0;

  upcomingTickets: Ticket[] = [];
  pastTickets: Ticket[] = [];
  tickets: Ticket[] = [];

  constructor(private ticketService: TicketService) {}

  ngOnInit() {
    let user: UserInfo = JSON.parse(localStorage.getItem('user')!);
    this.ticketService
      .getUserTickets(user.info.userId)
      .subscribe((res: Tickets) => {
        this.upcomingTickets = res.upcomingTickets;
        this.pastTickets = res.pastTickets;
        this.tickets = this.upcomingTickets;
      });
  }
}
