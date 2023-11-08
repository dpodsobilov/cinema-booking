import { Component, OnInit } from '@angular/core';
import { OrderService, SendData } from '../services/order.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css'],
})
export class OrderComponent implements OnInit {
  filmId: string = '';
  filmName: string = '';
  cinemaName: string = '';
  cinemaHallName: string = '';
  seatsQuantity: number = 0;
  seats: string = '';
  date: string = '';
  time: string = '';
  totalCost: number = 0;
  data: SendData = { userId: 0, sessionId: 0, placeAndCost: [] };
  dateTime: Date = new Date('00-00-00');
  constructor(
    private orderService: OrderService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.filmId = this.orderService.filmId;
    this.filmName = this.orderService.filmName;
    this.cinemaName = this.orderService.cinema;
    this.cinemaHallName = this.orderService.hall;
    this.dateTime = this.orderService.time;
    this.totalCost = this.orderService.totalCost;
    this.seats = this.orderService.seats;
    this.data = this.orderService.data;
    this.seatsQuantity = this.data.placeAndCost.length;
    this.time = this.getTime(String(this.dateTime));
    this.date = this.getDate(String(this.dateTime));
    console.log(this.orderService.data);
  }

  toOrder() {
    this.orderService.getTicket().subscribe(
      (response) => {
        if (response.status === 200) {
          this.router.navigate(['order-status']);
        }
      },
      (error) => {
        this.router.navigate(['error']);
      },
    );
  }

  getTime(date: string) {
    let converter = new Date(Date.parse(date));
    return (
      converter.getHours().toString() + ':' + converter.getMinutes().toString()
    );
  }
  getDate(date: string) {
    let converter = new Date(Date.parse(date));
    return (
      converter.getDate().toString() +
      ' ' +
      this.parseMonth(converter.getMonth()) +
      ' ' +
      converter.getFullYear().toString()
    );
  }
  parseMonth(month: number): string {
    let months: { [index: string]: string } = {
      '0': 'Января',
      '1': 'Февраля',
      '2': 'Марта',
      '3': 'Апреля',
      '4': 'Мая',
      '5': 'Июня',
      '6': 'Июля',
      '7': 'Августа',
      '8': 'Сентября',
      '9': 'Октября',
      '10': 'Ноября',
      '11': 'Декабря',
    };
    return months[month.toString()];
  }
}
