import { Component, OnInit } from '@angular/core';
import {
  AdminCinemas,
  AdminCinemasService,
  AdminHalls,
} from '../../services/admin/admin-cinemas.service';

@Component({
  selector: 'app-admin-cinemas',
  templateUrl: './admin-cinemas.component.html',
  styleUrls: ['./admin-cinemas.component.css'],
})
export class AdminCinemasComponent implements OnInit {
  cinemas: AdminCinemas[] = [];
  selectedString: any;
  constructor(public adminCinemasService: AdminCinemasService) {}

  ngOnInit(): void {
    this.adminCinemasService.getCinemas().subscribe((res: AdminCinemas[]) => {
      this.cinemas = res;
    });
  }

  selectStr(str: number) {
    this.selectedString = str;
    this.adminCinemasService.hallsForSelectedCinema = [];

    this.adminCinemasService.getHalls(str).subscribe((res: AdminHalls[]) => {
      this.adminCinemasService.hallsForSelectedCinema = res;
    });
  }
}
