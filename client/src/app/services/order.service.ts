import { Injectable } from '@angular/core';

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
  filmName: string = '';
  cinema: string = '';
  hall: string = '';
  time: Date = new Date('00-00-00');
  totalCost: number = 0;
  constructor() {}
}
