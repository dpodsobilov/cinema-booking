import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

export interface Place {
  placeId: number;
  placeTypeId: number;
  placeName: string;
  placeTypeName: string;
  color: string;
  cost: number;
}

@Injectable({
  providedIn: 'root',
})
export class PlacesService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  GetCinemaHall(sessionId: number): Observable<Place[][]> {
    return this.http.get<Place[][]>(
      this.baseUrl + '/Place?' + 'sessionId=' + sessionId,
    );
  }
}
