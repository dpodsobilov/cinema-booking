import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
export interface AdminCinema {
  cinemaId: number;
  cinemaName: string;
  cinemaAddress: string;
}

export interface AdminHall {
  cinemaHallId: number;
  cinemaHallName: string;
  cinemaHallTypeName: string;
  cinemaId: number;
}

export interface AdminHallCreation {
  cinemaHallName: string;
  cinemaHallTypeName: string;
  cinemaId: number;
}

@Injectable({
  providedIn: 'root',
})
export class AdminCinemasService {
  hallsForSelectedCinema: AdminHall[] = [];
  selectedStr: number = 0;
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getCinemas(): Observable<AdminCinema[]> {
    return this.http.get<AdminCinema[]>(this.baseUrl + '/Admin/Cinemas');
  }

  getHalls(cinemaId: number): Observable<AdminHall[]> {
    return this.http.get<AdminHall[]>(
      this.baseUrl + '/Admin/Halls?' + 'param=' + cinemaId,
    );
  }
  deleteCinema(cinemaId: number) {
    return this.http.delete(
      this.baseUrl + '/Admin/Cinema?' + 'cinemaId=' + cinemaId,
      {
        observe: 'response',
      },
    );
  }

  addHall(hall: AdminHallCreation) {
    return this.http.post(this.baseUrl + '/Admin/Hall?', hall, {
      observe: 'response',
    });
  }

  editHall(hall: AdminHall) {
    return this.http.put(this.baseUrl + '/Admin/Hall?', hall, {
      observe: 'response',
    });
  }

  deleteHall(cinemaHallId: number) {
    return this.http.delete(
      this.baseUrl + '/Admin/Hall?' + 'cinemaHallId=' + cinemaHallId,
      {
        observe: 'response',
      },
    );
  }
}
