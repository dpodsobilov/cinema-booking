import { Component, OnInit } from '@angular/core';
import {
  AdminCinema,
  AdminCinemasService,
  AdminHall,
} from '../../services/admin/admin-cinemas.service';
import { AdminSession } from '../../services/admin/admin-session.service';

@Component({
  selector: 'app-admin-cinemas',
  templateUrl: './admin-cinemas.component.html',
  styleUrls: ['./admin-cinemas.component.css'],
})
export class AdminCinemasComponent implements OnInit {
  cinemas: AdminCinema[] = [];
  selectedString: any;
  constructor(public adminCinemasService: AdminCinemasService) {}

  ngOnInit(): void {
    this.adminCinemasService.getCinemas().subscribe((res: AdminCinema[]) => {
      this.cinemas = res;
    });
  }

  selectStr(str: number) {
    this.selectedString = str;
    this.adminCinemasService.selectedStr = str;
    this.adminCinemasService.hallsForSelectedCinema = [];

    this.adminCinemasService.getHalls(str).subscribe((res: AdminHall[]) => {
      this.adminCinemasService.hallsForSelectedCinema = res;
    });
  }

  cinemaDelete(cinemaId: number) {
    this.adminCinemasService.deleteCinema(cinemaId).subscribe((response) => {
      if (response.status === 200) {
        this.adminCinemasService
          .getCinemas()
          .subscribe((res: AdminCinema[]) => {
            this.cinemas = res;
          });
        if (this.adminCinemasService.selectedStr === cinemaId) {
          this.adminCinemasService.hallsForSelectedCinema = [];
        }
      } else alert('Ошибка! Удаление не выполнено!');
    });
  }
}
