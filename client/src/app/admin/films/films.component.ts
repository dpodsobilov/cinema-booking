import { Component, OnInit } from '@angular/core';
import {
  AdminFilm,
  AdminFilmService,
} from '../../services/admin/admin-film.service';

@Component({
  selector: 'app-films',
  templateUrl: './films.component.html',
  styleUrls: ['./films.component.css'],
})
export class FilmsComponent implements OnInit {
  films: AdminFilm[] = [];

  constructor(private adminFilmService: AdminFilmService) {}

  ngOnInit(): void {
    this.adminFilmService.getFilms().subscribe((res: AdminFilm[]) => {
      this.films = res;
    });
  }

  deleteFilm(filmId: number) {
    this.adminFilmService.deleteFilm(filmId).subscribe((response) => {
      if (response.status === 200) {
        this.adminFilmService.getFilms().subscribe((res: AdminFilm[]) => {
          this.films = res;
        });
      } else alert('Ошибка! Удаление не выполнено!');
    });
  }
}
