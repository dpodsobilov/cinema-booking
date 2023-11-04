import { Component, OnInit } from '@angular/core';
import {
  Place,
  PlacesService,
} from '../services/places-service/places.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-places',
  templateUrl: './places.component.html',
  styleUrls: ['./places.component.css'],
})
export class PlacesComponent implements OnInit {
  matrix: Place[][] = [[]];
  sessionId: string = '';

  constructor(
    private route: ActivatedRoute,
    private placesService: PlacesService,
  ) {
    this.route.queryParams.subscribe((params) => {
      this.sessionId = params['sessionId'];
    });
  }

  ngOnInit(): void {
    this.placesService
      .GetCinemaHall(Number(this.sessionId))
      .subscribe((res: any) => {
        this.matrix = res.placeDtos;
      });
  }
}
