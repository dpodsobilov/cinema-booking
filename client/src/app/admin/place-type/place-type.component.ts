import { Component, OnInit } from '@angular/core';
import {
  PlaceType,
  AdminPlaceTypeCreation,
  PlaceTypeService,
} from '../../services/admin/place-type.service';
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

  addGenre(placeType: AdminPlaceTypeCreation) {
    this.newPlaceType = placeType;

    this.placeTypeService.addType(this.newPlaceType).subscribe((response) => {
      if (response.status === 200) {
        this.placeTypeService.getTypes().subscribe((res: PlaceType[]) => {
          this.types = res;
        });
      } else alert('Ошибка! Добавление не выполнено!');
    });
  }

  deleteType(typeId: number) {}

  editPlaceType($event: PlaceType) {}
}
