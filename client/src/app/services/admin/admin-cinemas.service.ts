import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
export interface AdminCinema {
  cinemaId: number;
  cinemaName: string;
  cinemaAddress: string;
}

export interface AdminCinemaCreation {
  cinemaName: string;
  address: string;
}

export interface AdminHall {
  cinemaHallId: number;
  cinemaHallName: string;
  cinemaHallTypeName: string;
  cinemaHallTypeId: number;
  cinemaId: number;
}

export interface AdminHallEditing {
  cinemaHallId: number;
  cinemaHallName: string;
  cinemaHallTypeId: number;
  cinemaId: number;
}

export interface AdminHallCreation {
  cinemaHallName: string;
  cinemaHallTypeId: number;
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

  addCinema(cinema: AdminCinemaCreation) {
    return this.http.post(this.baseUrl + '/Admin/Cinema?', cinema, {
      observe: 'response',
    });
  }

  editCinema(cinema: AdminCinema) {
    return this.http.put(this.baseUrl + '/Admin/Cinema?', cinema, {
      observe: 'response',
    });
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

  editHall(hall: AdminHallEditing) {
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
