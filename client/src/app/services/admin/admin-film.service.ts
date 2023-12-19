import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AdminFilm {
  filmId: number;
  filmName: string;
}

export interface AdminFilmCreation {
  filmName: string;
  hours: string;
  minutes: string;
  filmCoefficient: number;
  description: string;
  year: number;
  poster: string;
  genres: number[];
}

export interface AdminFilmEditing {
  filmId: number;
  filmName: string;
  hours: string;
  minutes: string;
  filmCoefficient: number;
  description: string;
  year: number;
  poster: string;
  genres: number[];
}

export interface AdminGenre {
  genreId: number;
  genreName: string;
}

export interface AdminGenreCreation {
  name: string;
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

  getFilm(filmId: number): Observable<AdminFilmEditing> {
    return this.http.get<AdminFilmEditing>(
      this.baseUrl + '/Admin/Film?' + 'filmId=' + filmId,
    );
  }

  addFilm(film: AdminFilmCreation) {
    return this.http.post(this.baseUrl + '/Admin/Film?', film, {
      observe: 'response',
    });
  }

  editFilm(film: AdminFilmEditing) {
    return this.http.put(this.baseUrl + '/Admin/Film?', film, {
      observe: 'response',
    });
  }

  deleteFilm(filmId: number) {
    return this.http.delete(
      this.baseUrl + '/Admin/Film?' + 'filmId=' + filmId,
      {
        observe: 'response',
      },
    );
  }

  addGenre(genre: AdminGenreCreation) {
    return this.http.post(this.baseUrl + '/Admin/Genre?', genre, {
      observe: 'response',
    });
  }

  editGenre(genre: AdminGenre) {
    return this.http.put(this.baseUrl + '/Admin/Genre?', genre, {
      observe: 'response',
    });
  }

  deleteGenre(genreId: number) {
    return this.http.delete(
      this.baseUrl + '/Admin/Genre?' + 'genreId=' + genreId,
      {
        observe: 'response',
      },
    );
  }
}
