import { Component, OnInit } from '@angular/core';
import {
  AdminCinemasService,
  AdminHall,
  AdminHallCreation,
} from '../../services/admin/admin-cinemas.service';
import { PlaceType } from '../../services/admin/place-type.service';

@Component({
  selector: 'app-halls',
  templateUrl: './halls.component.html',
  styleUrls: ['./halls.component.css'],
})
export class HallsComponent implements OnInit {
  isModalOpen: boolean = false;
  newHall: AdminHallCreation = {
    cinemaHallName: '',
    cinemaHallTypeName: '',
    cinemaId: 0,
  };
  oldHall: AdminHall = {
    cinemaHallId: 0,
    cinemaHallName: '',
    cinemaHallTypeName: '',
    cinemaId: 0,
  };
  isEditing: boolean = false;
  constructor(public adminCinemasService: AdminCinemasService) {}
  ngOnInit(): void {
    this.adminCinemasService.hallsForSelectedCinema = [];
  }

  openModal(oldHall?: AdminHall) {
    this.isModalOpen = true;

    if (oldHall != undefined) {
      this.oldHall = oldHall;
      this.isEditing = true;
    }
  }

  closeModal(isClose: boolean) {
    this.newHall = { cinemaHallName: '', cinemaHallTypeName: '', cinemaId: 0 };
    this.oldHall = {
      cinemaHallId: 0,
      cinemaHallName: '',
      cinemaHallTypeName: '',
      cinemaId: 0,
    };
    this.isModalOpen = !isClose;
    this.isEditing = false;
  }

  addHall(hall: AdminHallCreation) {
    this.newHall = hall;
    this.adminCinemasService.addHall(this.newHall).subscribe((response) => {
      if (response.status === 200) {
        // halls приходят прямо в шаблон из сервиса
        // this.adminCinemasService.getHalls().subscribe((res: AdminHall[]) => {
        //   this. = res;
        // });
      } else alert('Ошибка! Добавление не выполнено!');
    });
  }

  editHall(hall: AdminHall) {
    this.oldHall = hall;
    this.adminCinemasService.editHall(this.oldHall).subscribe((response) => {
      if (response.status === 200) {
        // this.placeTypeService.getTypes().subscribe((res: PlaceType[]) => {
        //   this.types = res;
        // });
      } else alert('Ошибка! Изменение не выполнено!');
    });
  }

  deleteHall(cinemaHallId: number) {
    this.adminCinemasService.deleteHall(cinemaHallId).subscribe((response) => {
      if (response.status === 200) {
        this.adminCinemasService
          .getHalls(this.adminCinemasService.selectedStr)
          .subscribe((res: AdminHall[]) => {
            this.adminCinemasService.hallsForSelectedCinema = res;
          });
      } else alert('Ошибка! Удаление не выполнено!');
    });
  }
}
