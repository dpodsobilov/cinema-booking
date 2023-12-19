import { Component, OnInit } from '@angular/core';
import {
  AdminFilm,
  AdminFilmCreation,
  AdminFilmEditing,
  AdminFilmService,
} from '../../services/admin/admin-film.service';
import { CustomError } from '../../services/admin/admin-cinemas.service';

@Component({
  selector: 'app-films',
  templateUrl: './films.component.html',
  styleUrls: ['./films.component.css'],
})
export class FilmsComponent implements OnInit {
  films: AdminFilm[] = [];
  isModalOpen: boolean = false;
  newFilm: AdminFilmCreation = {
    filmName: '',
    hours: '',
    minutes: '',
    filmCoefficient: 0,
    description: '',
    year: 0,
    poster: '',
    genres: [],
  };
  oldFilm: AdminFilmEditing = {
    filmId: 0,
    filmName: '',
    hours: '',
    minutes: '',
    filmCoefficient: 0,
    description: '',
    year: 0,
    poster: '',
    genres: [],
  };
  isEditing: boolean = false;

  constructor(private adminFilmService: AdminFilmService) {}

  ngOnInit(): void {
    this.adminFilmService.getFilms().subscribe((res: AdminFilm[]) => {
      this.films = res;
    });
  }

  openModal(oldFilm?: AdminFilm) {
    if (oldFilm != undefined) {
      this.adminFilmService.getFilm(oldFilm.filmId).subscribe({
        next: (response) => {
          this.oldFilm = response;
          this.isModalOpen = true;
        },
        error: (e: CustomError) => {
          alert('Ошибка: ' + e.error.Message);
        },
      });
      this.isEditing = true;
    } else {
      this.isModalOpen = true;
    }
  }

  closeModal(isClose: boolean) {
    this.newFilm = {
      filmName: '',
      hours: '',
      minutes: '',
      filmCoefficient: 0,
      description: '',
      year: 0,
      poster: '',
      genres: [],
    };
    this.oldFilm = {
      filmId: 0,
      filmName: '',
      hours: '',
      minutes: '',
      filmCoefficient: 0,
      description: '',
      year: 0,
      poster: '',
      genres: [],
    };
    this.isModalOpen = !isClose;
    this.isEditing = false;
  }

  addFilm(film: AdminFilmCreation) {
    this.newFilm = film;
    console.log(this.newFilm);
    this.adminFilmService.addFilm(this.newFilm).subscribe({
      next: (response) => {
        this.adminFilmService.getFilms().subscribe((res: AdminFilm[]) => {
          this.films = res;
        });
      },
      error: (e) => {
        alert('Ошибка: ' + e.error.Message);
      },
    });
  }

  editFilm(film: AdminFilmEditing) {
    this.oldFilm = film;
    console.log(this.oldFilm);
    this.adminFilmService.editFilm(film).subscribe({
      next: (response) => {
        this.adminFilmService.getFilms().subscribe((res: AdminFilm[]) => {
          this.films = res;
        });
      },
      error: (e) => {
        alert('Ошибка: ' + e.error.Message);
      },
    });
  }

  deleteFilm(filmId: number) {
    this.adminFilmService.deleteFilm(filmId).subscribe({
      next: (response) => {
        this.adminFilmService.getFilms().subscribe((res: AdminFilm[]) => {
          this.films = res;
        });
      },
      error: (e: CustomError) => {
        alert('Ошибка: ' + e.error.Message);
      },
    });
  }
}
