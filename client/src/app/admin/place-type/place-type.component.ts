import { Component, OnInit } from '@angular/core';
import {
  PlaceType,
  PlaceTypeService,
} from '../../services/admin/place-type.service';
@Component({
  selector: 'app-place-type',
  templateUrl: './place-type.component.html',
  styleUrls: ['./place-type.component.css'],
})
export class PlaceTypeComponent implements OnInit {
  types: PlaceType[] = [];
  constructor(private placeTypeService: PlaceTypeService) {}

  ngOnInit() {
    this.placeTypeService.getTypes().subscribe((res: PlaceType[]) => {
      this.types = res;
    });
  }

  deleteType(typeId: number) {}
}
