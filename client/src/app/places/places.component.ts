import { Component, OnInit } from '@angular/core';
import { Place, PlacesService } from '../services/places.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService, UserInfo } from '../services/auth.service';
import {
  OrderService,
  PlaceAndCost,
  SendData,
} from '../services/order.service';

export interface PlaceCost {
  id: number;
  price: number;
  name: string;
}

@Component({
  selector: 'app-places',
  templateUrl: './places.component.html',
  styleUrls: ['./places.component.css'],
})
export class PlacesComponent implements OnInit {
  matrix: Place[][] = [[]];
  sessionId: string = '';
  filmId: string = '';
  temp: PlaceCost[] = [];
  totalCost: number = 0;
  tempSelectedNames: string[] = [];
  selectedNames: string = '';

  placeAndCost: PlaceAndCost[] = [];
  // sendData:SendData = {userId: 0, sessionId: 0, placeAndCost: []}
  filmName: string = '';
  cinema: string = '';
  hall: string = '';
  time: Date = new Date('00-00-00');

  constructor(
    private route: ActivatedRoute,
    private placesService: PlacesService,
    private orderService: OrderService,
    private router: Router,
  ) {
    this.route.queryParams.subscribe((params) => {
      this.sessionId = params['sessionId'];
      this.filmId = params['filmId'];
      this.filmName = params['filmName'];
      this.cinema = params['cinema'];
      this.hall = params['hall'];
      this.time = params['time'];
    });
  }

  ngOnInit(): void {
    this.placesService
      .GetCinemaHall(Number(this.sessionId))
      .subscribe((res: any) => {
        this.matrix = res.placeDtos;
      });
  }

  goNext() {
    let userJSON = localStorage.getItem('user')!;
    let user: UserInfo = JSON.parse(userJSON);
    let userId = user.info.userId;
    this.orderService.data = {
      userId: userId,
      sessionId: +this.sessionId,
      placeAndCost: this.placeAndCost,
    };
    this.orderService.filmName = this.filmName;
    this.orderService.cinema = this.cinema;
    this.orderService.hall = this.hall;
    this.orderService.time = this.time;
    this.orderService.totalCost = this.totalCost;
    this.router.navigate(['/order']);
  }

  onPlaceSelect(placeId: number, placeName: string, cost: number) {
    this.selectedNames = '';
    let element = document.getElementById(
      'place-' + String(placeId),
    ) as HTMLElement;

    if (this.temp.find((t) => t.id === placeId) === undefined) {
      this.temp.push({ id: placeId, price: cost, name: placeName });
      element.style.border = '5px solid white';
      this.totalCost += cost;
      this.tempSelectedNames.push(placeName);

      this.placeAndCost.push({
        placeId: placeId,
        cost: cost,
      });
    } else {
      let index = this.temp.findIndex((item) => item.id === placeId);
      this.temp.splice(index, 1);

      element.style.border = '0px';
      this.totalCost -= cost;
      this.tempSelectedNames.splice(
        this.tempSelectedNames.indexOf(placeName),
        1,
      );

      let index2 = this.placeAndCost.findIndex(
        (item) => item.placeId === placeId,
      );
      this.placeAndCost.splice(index2, 1);
    }
    this.selectedNames = this.tempSelectedNames.join(', ');
  }
}
