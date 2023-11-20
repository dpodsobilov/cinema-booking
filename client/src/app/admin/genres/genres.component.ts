import { Component, OnInit } from '@angular/core';
import {
  AdminFilmService,
  AdminGenre,
} from '../../services/admin/admin-film.service';

@Component({
  selector: 'app-genres',
  templateUrl: './genres.component.html',
  styleUrls: ['./genres.component.css'],
})
export class GenresComponent implements OnInit {
  genres: AdminGenre[] = [
    {
      genreId: 1,
      genreName: 'Жанр',
    },
  ];

  constructor(private adminFilmService: AdminFilmService) {}

  ngOnInit(): void {
    // this.adminFilmService.getGenres().subscribe((res: AdminGenre[]) => {
    //   this.genres = res;
    // })
  }
}
