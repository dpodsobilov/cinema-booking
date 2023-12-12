import { Component, OnInit } from '@angular/core';
import {
  AdminCinema,
  AdminCinemaCreation,
  AdminCinemasService,
  AdminHall,
} from '../../services/admin/admin-cinemas.service';
import { AdminSession } from '../../services/admin/admin-session.service';
import { logBuilderStatusWarnings } from '@angular-devkit/build-angular/src/builders/browser-esbuild/builder-status-warnings';

@Component({
  selector: 'app-admin-cinemas',
  templateUrl: './admin-cinemas.component.html',
  styleUrls: ['./admin-cinemas.component.css'],
})
export class AdminCinemasComponent implements OnInit {
  cinemas: AdminCinema[] = [];
  selectedString: any;
  isModalOpen: boolean = false;
  newCinema: AdminCinemaCreation = { cinemaName: '', address: '' };
  oldCinema: AdminCinema = { cinemaId: 0, cinemaName: '', cinemaAddress: '' };
  isEditing: boolean = false;

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

  openModal(oldCinema?: AdminCinema) {
    this.isModalOpen = true;

    if (oldCinema != undefined) {
      this.oldCinema = oldCinema;
      this.isEditing = true;
    }
  }

  closeModal(isClose: boolean) {
    this.newCinema = { cinemaName: '', address: '' };
    this.oldCinema = { cinemaId: 0, cinemaName: '', cinemaAddress: '' };
    this.isModalOpen = !isClose;
    this.isEditing = false;
  }

  addCinema(cinema: AdminCinemaCreation) {
    this.newCinema = cinema;

    this.adminCinemasService.addCinema(this.newCinema).subscribe((response) => {
      if (response.status === 200) {
        this.adminCinemasService
          .getCinemas()
          .subscribe((res: AdminCinema[]) => {
            this.cinemas = res;
          });
      } else alert('Ошибка! Добавление не выполнено!');
    });
  }

  editCinema(cinema: AdminCinema) {
    this.oldCinema = cinema;

    this.adminCinemasService.editCinema(cinema).subscribe((response) => {
      if (response.status === 200) {
        this.adminCinemasService
          .getCinemas()
          .subscribe((res: AdminCinema[]) => {
            this.cinemas = res;
          });
        if (this.adminCinemasService.selectedStr === cinema.cinemaId) {
          this.adminCinemasService.hallsForSelectedCinema = [];
        }
      } else alert('Ошибка! Изменение не выполнено!');
    });
  }

  deleteCinema(cinemaId: number) {
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
