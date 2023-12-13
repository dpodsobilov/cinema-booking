import { Component, EventEmitter, Input, Output } from '@angular/core';
import {
  AdminSessionCreation,
  AdminSessionEditing,
} from '../../../services/admin/admin-session.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  AdminFilm,
  AdminFilmService,
} from '../../../services/admin/admin-film.service';
import {
  AdminCinema,
  AdminCinemasService,
  AdminHall,
} from '../../../services/admin/admin-cinemas.service';

@Component({
  selector: 'app-session-modal',
  templateUrl: './session-modal.component.html',
  styleUrls: ['./session-modal.component.css'],
})
export class SessionModalComponent {
  @Input() isEditing = false;
  @Input() oldSession: AdminSessionEditing = {
    sessionId: 0,
    dataTimeSession: '',
    dataTimeCoefficient: 0,
    cinemaHallId: 0,
    filmId: 0,
  };
  @Output() closeEvent = new EventEmitter<boolean>();
  @Output() saveEvent = new EventEmitter<AdminSessionCreation>();
  @Output() editEvent = new EventEmitter<AdminSessionEditing>();
  films: AdminFilm[] = [];
  cinemas: AdminCinema[] = [];
  selectedString: any;

  sessionForm: FormGroup = null!;

  constructor(
    private adminFilmService: AdminFilmService,
    public adminCinemasService: AdminCinemasService,
  ) {}
  ngOnInit() {
    this.adminFilmService.getFilms().subscribe({
      next: (response: AdminFilm[]) => {
        this.films = response;
      },
      error: (e) => {
        alert('Ошибка загрузки фильмов');
      },
    });

    this.adminCinemasService.getCinemas().subscribe({
      next: (response: AdminCinema[]) => {
        this.cinemas = response;
      },
      error: (e) => {
        alert('ОШибка загрузки кинотеатров');
      },
    });

    this.sessionForm = new FormGroup({
      filmIdControl: new FormControl(this.oldSession.filmId, [
        Validators.required,
      ]),
      cinemaIdControl: new FormControl('', [Validators.required]),
      cinemaHallIdControl: new FormControl(this.oldSession.cinemaHallId, [
        Validators.required,
      ]),
      coefficientControl: new FormControl(this.oldSession.dataTimeCoefficient, [
        Validators.required,
        Validators.min(1),
      ]),
      dateControl: new FormControl(
        this.oldSession.dataTimeSession.substring(0, 10),
        [Validators.required, Validators.pattern('^\\d{4}-\\d{2}-\\d{2}$')],
      ),
      timeControl: new FormControl(
        this.oldSession.dataTimeSession.substring(11, 16),
        [Validators.required, Validators.pattern('^\\d{2}:\\d{2}$')],
      ),
    });
  }

  errorMessage = '';

  selectStr(str: number) {
    this.selectedString = str;
    this.adminCinemasService.selectedStr = str;
    this.adminCinemasService.hallsForSelectedCinema = [];

    this.adminCinemasService.getHalls(str).subscribe((res: AdminHall[]) => {
      this.adminCinemasService.hallsForSelectedCinema = res;
    });
  }

  makeDateTime(date: string, time: string): string {
    const res = new Date(date + 'T' + time + ':00Z');
    return res.toISOString();
  }

  get filmId() {
    return this.sessionForm.controls['filmIdControl'];
  }
  get cinemaId() {
    return this.sessionForm.controls['cinemaIdControl'];
  }
  get cinemaHallId() {
    return this.sessionForm.controls['cinemaHallIdControl'];
  }
  get coefficient() {
    return this.sessionForm.controls['coefficientControl'];
  }
  get date() {
    return this.sessionForm.controls['dateControl'];
  }
  get time() {
    return this.sessionForm.controls['timeControl'];
  }

  close() {
    this.closeEvent.emit(true);
  }

  onSubmit() {
    const filmId = this.sessionForm.get('filmIdControl')?.value!;
    const cinemaHallId = this.sessionForm.get('cinemaHallIdControl')?.value!;
    const coefficient = this.sessionForm.get('coefficientControl')?.value!;
    const date = this.sessionForm.get('dateControl')?.value!;
    const time = this.sessionForm.get('timeControl')?.value!;
    const isoDateTime = this.makeDateTime(date, time);

    if (this.oldSession.sessionId != 0) {
      this.editEvent.emit({
        sessionId: this.oldSession.sessionId,
        filmId: filmId,
        dataTimeCoefficient: coefficient,
        cinemaHallId: cinemaHallId,
        dataTimeSession: isoDateTime,
      });
    } else {
      this.saveEvent.emit({
        filmId: filmId,
        dataTimeCoefficient: coefficient,
        cinemaHallId: cinemaHallId,
        dataTimeSession: isoDateTime,
      });
    }
    this.close();
  }
}
