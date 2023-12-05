import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminFilm, AdminGenre } from './admin-film.service';

export interface FilmStatsString {
  filmName: string;
  orderedTickets: number;
  totalTickets: number;
  percentage: number;
}

@Injectable({
  providedIn: 'root',
})
export class AdminTicketsService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getStats(): Observable<FilmStatsString[]> {
    return this.http.get<FilmStatsString[]>(this.baseUrl + '/Admin/Stats');
  }
}
