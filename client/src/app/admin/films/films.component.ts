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
  films: AdminFilm[] = [
    {
      filmName: 'Три богатыря и Шамаханская царица',
      filmId: 2,
    },
  ];

  constructor(private adminFilmService: AdminFilmService) {}

  ngOnInit(): void {
    // this.adminFilmService.getFilms().subscribe((res: AdminFilm[]) => {
    //   this.films = res;
    // });
  }
}
