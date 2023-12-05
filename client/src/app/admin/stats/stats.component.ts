import { Component, OnInit } from '@angular/core';
import {
  AdminTicketsService,
  FilmStatsString,
} from '../../services/admin/admin-tickets.service';
@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.css'],
})
export class StatsComponent implements OnInit {
  statistics: FilmStatsString[] = [];

  constructor(private adminStatsService: AdminTicketsService) {}

  ngOnInit(): void {
    this.adminStatsService.getStats().subscribe((res: FilmStatsString[]) => {
      this.statistics = res;
    });
  }
}
