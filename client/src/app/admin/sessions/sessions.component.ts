import { Component, OnInit } from '@angular/core';
import {
  AdminSession,
  AdminSessionCreation,
  AdminSessionEditing,
  AdminSessionService,
} from '../../services/admin/admin-session.service';
import { find } from 'rxjs';

@Component({
  selector: 'app-sessions',
  templateUrl: './sessions.component.html',
  styleUrls: ['./sessions.component.css'],
})
export class SessionsComponent implements OnInit {
  sessions: AdminSession[] = [];
  servRequest: AdminSession[] = [];

  films: string[] = [];
  isModalOpen: boolean = false;
  newSession: AdminSessionCreation = {
    dataTimeSession: '',
    dataTimeCoefficient: 0,
    cinemaHallId: 0,
    filmId: 0,
  };
  oldSession: AdminSessionEditing = {
    sessionId: 0,
    dataTimeSession: '',
    dataTimeCoefficient: 0,
    cinemaHallId: 0,
    filmId: 0,
  };
  isEditing: boolean = false;
  constructor(private adminSessionService: AdminSessionService) {}
  ngOnInit() {
    this.adminSessionService.getSessions().subscribe((res: AdminSession[]) => {
      this.servRequest = res;
      this.sessions = res;
      this.createFilters();
    });
  }

  openModal(oldSession?: AdminSession) {
    if (oldSession != undefined) {
      this.oldSession = {
        sessionId: oldSession.sessionId,
        filmId: oldSession.filmId,
        cinemaHallId: oldSession.cinemaHallId,
        dataTimeCoefficient: oldSession.dataTimeCoefficient,
        dataTimeSession: oldSession.dataTimeSession,
      };
      this.isModalOpen = true;
      this.isEditing = true;
    } else {
      this.isModalOpen = true;
    }
  }

  closeModal(isClose: boolean) {
    this.newSession = {
      dataTimeSession: '',
      dataTimeCoefficient: 0,
      cinemaHallId: 0,
      filmId: 0,
    };
    this.oldSession = {
      sessionId: 0,
      dataTimeSession: '',
      dataTimeCoefficient: 0,
      cinemaHallId: 0,
      filmId: 0,
    };
    this.isModalOpen = !isClose;
    this.isEditing = false;
  }

  createFilters() {
    for (let i = 0; i < this.servRequest.length; i++) {
      if (!this.films.includes(this.servRequest[i].filmName)) {
        this.films.push(this.servRequest[i].filmName);
      }
    }
  }

  filmFilter(event: any) {
    if (event.target.value === 'All') {
      this.sessions = this.servRequest;
    } else {
      this.sessions = [];
      for (let i = 0; i < this.servRequest.length; i++) {
        if (this.servRequest[i].filmName === event.target.value) {
          this.sessions.push(this.servRequest[i]);
        }
      }
    }
  }

  addSession(session: AdminSessionCreation) {
    this.newSession = session;
    console.log(this.newSession);
    this.adminSessionService.addSession(this.newSession).subscribe({
      next: (response) => {
        this.adminSessionService
          .getSessions()
          .subscribe((res: AdminSession[]) => {
            this.sessions = res;
          });
      },
      error: (e) => {
        alert('Ошибка добавления');
      },
    });
  }

  editSession(session: AdminSessionEditing) {
    this.oldSession = session;
    this.adminSessionService.editSession(this.oldSession).subscribe({
      next: (response) => {
        this.adminSessionService
          .getSessions()
          .subscribe((res: AdminSession[]) => {
            this.sessions = res;
          });
      },
      error: (e) => {
        alert('Ошибка изменения');
      },
    });
  }

  deleteSession(sessionId: number) {
    this.adminSessionService.deleteSession(sessionId).subscribe((response) => {
      if (response.status === 200) {
        this.adminSessionService
          .getSessions()
          .subscribe((res: AdminSession[]) => {
            this.sessions = res;
          });
      } else alert('Ошибка! Удаление не выполнено!');
    });
  }
}
