import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminFilm } from './admin-film.service';

export interface AdminSession {
  sessionId: number;
  filmName: string;
  cinemaName: string;
  cinemaHallName: string;
  sessionDate: string;
  sessionTime: string;
}
@Injectable({
  providedIn: 'root',
})
export class AdminSessionService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getSessions(): Observable<AdminSession[]> {
    return this.http.get<AdminSession[]>(this.baseUrl + '/Admin/Sessions');
  }

  deleteSession(sessionId: number) {
    return this.http.delete(
      this.baseUrl + '/Admin/Session?' + 'sessionId=' + sessionId,
      {
        observe: 'response',
      },
    );
  }
}
