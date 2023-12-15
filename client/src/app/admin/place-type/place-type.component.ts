import { Component, OnInit } from '@angular/core';
import {
  PlaceType,
  AdminPlaceTypeCreation,
  PlaceTypeService,
} from '../../services/admin/place-type.service';
import { CustomError } from '../../services/admin/admin-cinemas.service';
@Component({
  selector: 'app-place-type',
  templateUrl: './place-type.component.html',
  styleUrls: ['./place-type.component.css'],
})
export class PlaceTypeComponent implements OnInit {
  types: PlaceType[] = [];
  isModalOpen: boolean = false;
  newPlaceType: AdminPlaceTypeCreation = { color: '', name: '', cost: 0 };
  oldPlaceType: PlaceType = { typeId: 0, color: '', name: '', cost: 0 };
  isEditing: boolean = false;

  constructor(private placeTypeService: PlaceTypeService) {}

  ngOnInit() {
    this.placeTypeService.getTypes().subscribe((res: PlaceType[]) => {
      this.types = res;
    });
  }

  openModal(oldPlaceType?: PlaceType) {
    this.isModalOpen = true;

    if (oldPlaceType != undefined) {
      this.oldPlaceType = oldPlaceType;
      this.isEditing = true;
    }
  }

  closeModal(isClose: boolean) {
    this.newPlaceType = { color: '', name: '', cost: 0 };
    this.oldPlaceType = { typeId: 0, color: '', name: '', cost: 0 };
    this.isModalOpen = !isClose;
    this.isEditing = false;
  }

  addPlaceType(placeType: AdminPlaceTypeCreation) {
    this.newPlaceType = placeType;

    this.placeTypeService.addType(this.newPlaceType).subscribe({
      next: (response) => {
        this.placeTypeService.getTypes().subscribe((res: PlaceType[]) => {
          this.types = res;
        });
      },
      error: (e: CustomError) => {
        alert('Ошибка: ' + e.error.Message);
      },
    });
  }

  editPlaceType(placeType: PlaceType) {
    this.oldPlaceType = placeType;
    this.placeTypeService.editType(placeType).subscribe({
      next: (response) => {
        this.placeTypeService.getTypes().subscribe((res: PlaceType[]) => {
          this.types = res;
        });
      },
      error: (e: CustomError) => {
        alert('Ошибка: ' + e.error.Message);
      },
    });
  }

  deleteType(placeTypeId: number) {
    this.placeTypeService.deleteType(placeTypeId).subscribe({
      next: (response) => {
        this.placeTypeService.getTypes().subscribe((res: PlaceType[]) => {
          this.types = res;
        });
      },
      error: (e: CustomError) => {
        alert('Ошибка: ' + e.error.Message);
      },
    });
  }
}
