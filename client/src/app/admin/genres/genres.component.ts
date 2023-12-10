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
  genre: AdminGenreCreation = { name: '' };

  constructor(private adminFilmService: AdminFilmService) {}

  ngOnInit(): void {
    this.adminFilmService.getGenres().subscribe((res: AdminGenre[]) => {
      this.genres = res;
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

  openModal() {
    this.isModalOpen = true;
  }

  closeModal(event: boolean) {
    this.isModalOpen = false;
  }

  addGenre(genre: AdminGenreCreation) {
    this.genre = genre;
    this.adminFilmService.addGenre(this.genre).subscribe((response) => {
      if (response.status === 200) {
        this.adminFilmService.getGenres().subscribe((res: AdminGenre[]) => {
          this.genres = res;
        });
      } else alert('Ошибка! Добавление не выполнено!');
    });
  }
}
