import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminFilm } from './admin-film.service';

export interface AdminSession {
  sessionId: number;
  filmId: number;
  filmName: string;
  cinemaName: string;
  dataTimeCoefficient: number;
  cinemaHallId: number;
  cinemaHallName: string;
  sessionDate: string;
  sessionTime: string;
  dataTimeSession: string;
}

export interface AdminSessionCreation {
  dataTimeSession: string;
  dataTimeCoefficient: number;
  filmId: number;
  cinemaHallId: number;
}

export interface AdminSessionEditing {
  sessionId: number;
  dataTimeSession: string;
  dataTimeCoefficient: number;
  filmId: number;
  cinemaHallId: number;
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

  addSession(session: AdminSessionCreation) {
    return this.http.post(this.baseUrl + '/Admin/Session?', session, {
      observe: 'response',
    });
  }

  editSession(session: AdminSessionEditing) {
    return this.http.put(this.baseUrl + '/Admin/Session?', session, {
      observe: 'response',
    });
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
