import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Film } from '../film.service';

export interface AdminFilm {
  filmId: number;
  filmName: string;
}

export interface AdminGenre {
  genreId: number;
  genreName: string;
}

@Injectable({
  providedIn: 'root',
})
export class AdminFilmService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getFilms(): Observable<AdminFilm[]> {
    return this.http.get<AdminFilm[]>(this.baseUrl + '/Admin/Films');
  }

  getGenres(): Observable<AdminGenre[]> {
    return this.http.get<AdminGenre[]>(this.baseUrl + '/Admin/Genres');
  }
}
