import { Component, Input } from '@angular/core';
import { Ticket } from '../../services/ticket.service';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css'],
})
export class TicketComponent {
  @Input('ticket') ticket: Ticket = {
    ticketId: 0,
    date: '',
    cinemaHallName: '',
    cinemaName: '',
    filmName: '',
    placeName: '',
    placeTypeName: '',
  };

  @Input('isUpcoming') isUpcoming = false;

  getDate(date: string) {
    let converter = new Date(Date.parse(date));
    return (
      this.parseDayOfWeek(converter.getDay()) +
      ', ' +
      converter.getDate().toString() +
      ' ' +
      this.parseMonth(converter.getMonth()) +
      ' ' +
      converter.getFullYear().toString()
    );
  }
  getTime(date: string) {
    let converter = new Date(Date.parse(date));
    return (
      converter.getHours().toString() + ':' + converter.getMinutes().toString()
    );
  }
  parseMonth(month: number): string {
    let months: { [index: string]: string } = {
      '0': 'Янв',
      '1': 'Фев',
      '2': 'Мар',
      '3': 'Апр',
      '4': 'Май',
      '5': 'Июн',
      '6': 'Июл',
      '7': 'Авг',
      '8': 'Сен',
      '9': 'Окт',
      '10': 'Ноя',
      '11': 'Дек',
    };
    return months[month.toString()];
  }

  parseDayOfWeek(day: number): string {
    let days: { [index: string]: string } = {
      '0': 'Вс',
      '1': 'Пн',
      '2': 'Вт',
      '3': 'Ср',
      '4': 'Чт',
      '5': 'Пт',
      '6': 'Сб',
    };
    return days[day.toString()];
  }
}
