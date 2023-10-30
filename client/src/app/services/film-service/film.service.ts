import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

export interface Film{
  filmId: number
  filmName: string
  duration: string
  description: string
  poster: string
  filmGenres: string[]
}
export interface Schedule{
  cinemaId: number
  cinemaName: string
  cinemaHallId: number
  cinemaHallName: string
  sessionId: number
  dataTimeSession: string
}
@Injectable({
  providedIn: 'root'
})
export class FilmService {
  constructor(private http: HttpClient, @Inject('BASE_API_URL') private baseUrl: string) { }
  GetFilmInfo(filmId: number): Observable<Film>{
    return this.http.get<Film>(this.baseUrl + '/Film?' + 'param=' + filmId)
  }

  GetSchedule(filmId: number): Observable<Schedule[]>{
    return this.http.post<Schedule[]>(this.baseUrl + '/Film', filmId)
  }
}
