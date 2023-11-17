import { Component, OnInit } from '@angular/core';
import { Place, PlacesService } from '../../services/places.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService, UserInfo } from '../../services/auth.service';
import {
  OrderService,
  PlaceAndCost,
  SendData,
} from '../../services/order.service';

export interface PlaceCost {
  id: number;
  price: number;
  name: string;
}

export interface Legend {
  typeName: string;
  color: string;
  cost: number;
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
  legend: Legend[] = [];
  isChild: boolean = false;
  childCoeff: number = 1;
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
        this.buildLegend();
      });
  }

  buildLegend() {
    for (let i = 0; i < this.matrix.length; i++) {
      for (let j = 0; j < this.matrix[i].length; j++) {
        if (
          this.legend.find(
            (l) => l.typeName === this.matrix[i][j].placeTypeName,
          ) === undefined &&
          this.matrix[i][j].placeTypeName !== 'default'
        ) {
          this.legend.push({
            typeName: this.matrix[i][j].placeTypeName,
            color: this.matrix[i][j].color,
            cost: this.matrix[i][j].cost,
          });
        }
      }
    }
  }

  selectChild() {
    if (!this.isChild) {
      alert(
        'При оплате билета на кассе необходимо предъявить свидетельство о рождении ребенка! \n Скидка предоставляется детям до 10 лет включительно',
      );
    }
  }

  goNext() {
    if (localStorage.getItem('isAuthenticated') === 'true') {
      if (this.placeAndCost.length !== 0) {
        let userJSON = localStorage.getItem('user')!;
        let user: UserInfo = JSON.parse(userJSON);
        let userId = user.info.userId;
        this.orderService.data = {
          userId: userId,
          sessionId: +this.sessionId,
          placeAndCost: this.placeAndCost,
        };
        this.orderService.filmId = this.filmId;
        this.orderService.filmName = this.filmName;
        this.orderService.cinema = this.cinema;
        this.orderService.hall = this.hall;
        this.orderService.time = this.time;
        this.orderService.totalCost = this.totalCost;
        this.orderService.seats = this.selectedNames;
        this.router.navigate(['/order']);
      } else {
        alert('Необходимо выбрать место!');
      }
    } else {
      alert('Необходимо войти в систему!');
      this.router.navigate(['/login']);
    }
  }

  onPlaceSelect(placeId: number, placeName: string, cost: number) {
    if (this.isChild) {
      this.childCoeff = 0.5;
    } else {
      this.childCoeff = 1;
    }
    let element = document.getElementById(
      'place-' + String(placeId),
    ) as HTMLElement;

    if (this.temp.find((t) => t.id === placeId) === undefined) {
      if (this.tempSelectedNames.length < 5) {
        this.temp.push({ id: placeId, price: cost, name: placeName });
        if (!this.isChild) {
          element.style.border = '4px solid white';
        } else {
          element.style.border = '4px dashed white';
        }
        this.totalCost += cost * this.childCoeff;
        this.tempSelectedNames.push(placeName);
        this.placeAndCost.push({
          placeId: placeId,
          price: cost * this.childCoeff,
        });
      } else if (this.tempSelectedNames.length >= 5) {
        alert('Невозможно выбрать более 5 мест!');
      }
    } else {
      let index = this.temp.findIndex((item) => item.id === placeId);
      let indexindexov = this.placeAndCost.findIndex(
        (p) => p.placeId === placeId,
      );
      let newCost = this.placeAndCost[indexindexov].price;
      this.temp.splice(index, 1);

      element.style.border = '0px';
      this.totalCost -= newCost;
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
