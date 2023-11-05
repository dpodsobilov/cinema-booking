import { Place } from './places.service';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Schedule } from './film.service';

export interface SendData {
  userId: number;
  sessionId: number;
  placeAndCost: PlaceAndCost[];
}
export interface PlaceAndCost {
  placeId: number;
  cost: number;
}
@Injectable({
  providedIn: 'root',
})
export class OrderService {
  data: SendData = { userId: 0, sessionId: 0, placeAndCost: [] };
  filmId: string = '';
  filmName: string = '';
  cinema: string = '';
  hall: string = '';
  time: Date = new Date('00-00-00');
  totalCost: number = 0;
  seats: string = '';

  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getTicket(): Observable<HttpResponse<Object>> {
    return this.http.post(this.baseUrl + '/Order', this.data, {
      observe: 'response',
    });
  }
}
