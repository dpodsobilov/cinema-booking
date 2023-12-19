import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  PlaceType,
  AdminPlaceTypeCreation,
} from '../../../services/admin/place-type.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-place-type-modal',
  templateUrl: './place-type-modal.component.html',
  styleUrls: ['./place-type-modal.component.css'],
})
export class PlaceTypeModalComponent implements OnInit {
  @Input() isEditing = false;
  @Input() oldPlaceType: PlaceType = {
    typeId: 0,
    color: '',
    name: '',
    cost: 0,
  };
  @Output() closeEvent = new EventEmitter<boolean>();
  @Output() saveEvent = new EventEmitter<AdminPlaceTypeCreation>();
  @Output() editEvent = new EventEmitter<PlaceType>();

  placeTypeForm: FormGroup = null!;

  ngOnInit() {
    this.placeTypeForm = new FormGroup({
      placeTypeNameControl: new FormControl(this.oldPlaceType.name, [
        Validators.required,
      ]),
      placeTypeColorControl: new FormControl(this.oldPlaceType.color, [
        Validators.required,
      ]),
      placeTypeCostControl: new FormControl(this.oldPlaceType.cost, [
        Validators.required,
        Validators.min(100),
      ]),
    });
  }

  errorMessage = '';

  get name() {
    return this.placeTypeForm.controls['placeTypeNameControl'];
  }

  get color() {
    return this.placeTypeForm.controls['placeTypeColorControl'];
  }

  get cost() {
    return this.placeTypeForm.controls['placeTypeCostControl'];
  }

  close() {
    this.closeEvent.emit(true);
  }

  onSubmit() {
    const placeTypeName = this.placeTypeForm.get('placeTypeNameControl')
      ?.value!;
    const placeTypeColor = this.placeTypeForm.get('placeTypeColorControl')
      ?.value!;
    const placeTypeCost = this.placeTypeForm.get('placeTypeCostControl')
      ?.value!;

    // если старое имя есть, значит мы редактируем
    if (this.oldPlaceType.typeId != 0) {
      this.editEvent.emit({
        typeId: this.oldPlaceType.typeId,
        name: placeTypeName,
        color: placeTypeColor,
        cost: placeTypeCost,
      });
    } else {
      this.saveEvent.emit({
        name: placeTypeName,
        color: placeTypeColor,
        cost: placeTypeCost,
      });
    }
    this.close();
  }
}
