import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

export interface HomePageFilm {
  filmId: number;
  filmName: string;
  poster: string;
}
export interface CinemaFilms {
  cinemaId: number;
  cinemaName: string;
  films: HomePageFilm[];
}

@Injectable({
  providedIn: 'root',
})
export class HomepageService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  GetHomePageFilms(): Observable<CinemaFilms[]> {
    return this.http.get<CinemaFilms[]>(this.baseUrl + '/HomePage');
  }
}
