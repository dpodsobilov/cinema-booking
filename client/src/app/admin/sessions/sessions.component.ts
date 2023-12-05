import { Component, OnInit } from '@angular/core';
import {
  AdminSession,
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
  constructor(private adminSessionService: AdminSessionService) {}
  ngOnInit() {
    this.adminSessionService.getSessions().subscribe((res: AdminSession[]) => {
      this.servRequest = res;
      this.sessions = res;
      this.createFilters();
    });
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
