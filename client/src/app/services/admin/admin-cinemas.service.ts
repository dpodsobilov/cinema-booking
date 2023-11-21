import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
export interface AdminCinemas {
  cinemaId: number;
  cinemaName: string;
  cinemaAddress: string;
}

export interface AdminHalls {
  cinemaHallId: number;
  cinemaHallName: string;
  cinemaHallTypeName: string;
  cinemaId: number;
}

@Injectable({
  providedIn: 'root',
})
export class AdminCinemasService {
  hallsForSelectedCinema: AdminHalls[] = [];
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getCinemas(): Observable<AdminCinemas[]> {
    return this.http.get<AdminCinemas[]>(this.baseUrl + '/Admin/Cinemas');
  }

  getHalls(cinemaId: number): Observable<AdminHalls[]> {
    return this.http.get<AdminHalls[]>(
      this.baseUrl + '/Admin/Halls?' + 'param=' + cinemaId,
    );
  }
}
