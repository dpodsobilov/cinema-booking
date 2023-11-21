import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@aspnet/signalr';

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
  placesHubConnection: signalR.HubConnection =
    new signalR.HubConnectionBuilder()
      .withUrl(this.baseUrl + '/Places', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .build();

  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  GetCinemaHall(sessionId: number): Observable<Place[][]> {
    return this.http.get<Place[][]>(
      this.baseUrl + '/Place?' + 'sessionId=' + sessionId,
    );
  }

  startConnection = () => {
    this.placesHubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.baseUrl + '/Places', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .build();

    this.placesHubConnection
      .start()
      .then()
      .catch((err) =>
        console.log('Error while starting hub connection: ' + err),
      );
  };
}
