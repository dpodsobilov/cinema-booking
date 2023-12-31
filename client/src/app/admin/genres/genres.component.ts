import { Component, OnInit } from '@angular/core';
import {
  AdminFilm,
  AdminFilmService,
  AdminGenre,
  AdminGenreCreation,
} from '../../services/admin/admin-film.service';
import { CustomError } from '../../services/admin/admin-cinemas.service';

@Component({
  selector: 'app-genres',
  templateUrl: './genres.component.html',
  styleUrls: ['./genres.component.css'],
})
export class GenresComponent implements OnInit {
  genres: AdminGenre[] = [];
  isModalOpen: boolean = false;
  newGenre: AdminGenreCreation = { name: '' };
  oldGenre: AdminGenre = { genreId: 0, genreName: '' };
  isEditing: boolean = false;

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
      this.isEditing = true;
    }
  }

  closeModal(isClose: boolean) {
    this.newGenre = { name: '' };
    this.oldGenre = { genreId: 0, genreName: '' };
    this.isModalOpen = !isClose;
    this.isEditing = false;
  }

  addGenre(genre: AdminGenreCreation) {
    this.newGenre = genre;
    this.adminFilmService.addGenre(this.newGenre).subscribe({
      next: (response) => {
        this.adminFilmService.getGenres().subscribe((res: AdminGenre[]) => {
          this.genres = res;
        });
      },
      error: (e: CustomError) => {
        alert('Ошибка: ' + e.error.Message);
      },
    });
  }

  editGenre(genre: AdminGenre) {
    this.oldGenre = genre;
    this.adminFilmService.editGenre(genre).subscribe({
      next: (response) => {
        this.adminFilmService.getGenres().subscribe((res: AdminGenre[]) => {
          this.genres = res;
        });
      },
      error: (e: CustomError) => {
        alert('Ошибка: ' + e.error.Message);
      },
    });
  }

  deleteGenre(genreId: number) {
    this.adminFilmService.deleteGenre(genreId).subscribe({
      next: (response) => {
        this.adminFilmService.getGenres().subscribe((res: AdminGenre[]) => {
          this.genres = res;
        });
      },
      error: (e: CustomError) => {
        alert('Ошибка: ' + e.error.Message);
      },
    });
  }
}
