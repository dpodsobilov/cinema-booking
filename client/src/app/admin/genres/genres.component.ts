import { Component, OnInit } from '@angular/core';
import {
  AdminFilm,
  AdminFilmService,
  AdminGenre,
  AdminGenreCreation,
} from '../../services/admin/admin-film.service';

@Component({
  selector: 'app-genres',
  templateUrl: './genres.component.html',
  styleUrls: ['./genres.component.css'],
})
export class GenresComponent implements OnInit {
  genres: AdminGenre[] = [];
  isModalOpen: boolean = false;
  newGenre: AdminGenreCreation = { name: '' };
  oldGenre: AdminGenre = { genreId: null!, genreName: null! };

  constructor(private adminFilmService: AdminFilmService) {}

  ngOnInit(): void {
    this.adminFilmService.getGenres().subscribe((res: AdminGenre[]) => {
      this.genres = res;
    });
  }

  openModal(oldGenre?: AdminGenre) {
    this.isModalOpen = true;

    if (oldGenre != undefined) {
      this.oldGenre = oldGenre;
    }
  }

  closeModal(isClose: boolean) {
    this.newGenre = null!;
    this.oldGenre = null!;
    this.isModalOpen = !isClose;
  }

  addGenre(genre: AdminGenreCreation) {
    this.newGenre = genre;
    this.adminFilmService.addGenre(this.newGenre).subscribe((response) => {
      if (response.status === 200) {
        this.adminFilmService.getGenres().subscribe((res: AdminGenre[]) => {
          this.genres = res;
        });
      } else alert('Ошибка! Добавление не выполнено!');
    });
  }

  editGenre(genre: AdminGenre) {
    this.oldGenre = genre;
    this.adminFilmService.editGenre(genre).subscribe((response) => {
      if (response.status === 200) {
        this.adminFilmService.getGenres().subscribe((res: AdminGenre[]) => {
          this.genres = res;
        });
      } else alert('Ошибка! Изменение не выполнено!');
    });
  }

  deleteGenre(genreId: number) {
    this.adminFilmService.deleteGenre(genreId).subscribe((response) => {
      if (response.status === 200) {
        this.adminFilmService.getGenres().subscribe((res: AdminGenre[]) => {
          this.genres = res;
        });
      } else alert('Ошибка! Удаление не выполнено!');
    });
  }
}
