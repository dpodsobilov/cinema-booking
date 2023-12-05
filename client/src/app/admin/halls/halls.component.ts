import { Component, OnInit } from '@angular/core';
import {
  AdminCinemas,
  AdminCinemasService,
  AdminHalls,
} from '../../services/admin/admin-cinemas.service';

@Component({
  selector: 'app-halls',
  templateUrl: './halls.component.html',
  styleUrls: ['./halls.component.css'],
})
export class HallsComponent implements OnInit {
  constructor(public adminCinemasService: AdminCinemasService) {}
  ngOnInit(): void {
    this.adminCinemasService.hallsForSelectedCinema = [];
  }

  hallDelete(cinemaHallId: number) {
    this.adminCinemasService.deleteHall(cinemaHallId).subscribe((response) => {
      if (response.status === 200) {
        this.adminCinemasService
          .getHalls(this.adminCinemasService.selectedStr)
          .subscribe((res: AdminHalls[]) => {
            this.adminCinemasService.hallsForSelectedCinema = res;
          });
      } else alert('Ошибка! Удаление не выполнено!');
    });
  }
}
