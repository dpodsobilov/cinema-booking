import { Component, EventEmitter, Input, Output } from '@angular/core';
import {
  AdminCinema,
  AdminCinemaCreation,
} from '../../../services/admin/admin-cinemas.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-cinema-modal',
  templateUrl: './cinema-modal.component.html',
  styleUrls: ['./cinema-modal.component.css'],
})
export class CinemaModalComponent {
  @Input() isEditing = false;
  @Input() oldCinema: AdminCinema = {
    cinemaId: 0,
    cinemaName: '',
    cinemaAddress: '',
  };
  @Output() closeEvent = new EventEmitter<boolean>();
  @Output() saveEvent = new EventEmitter<AdminCinemaCreation>();
  @Output() editEvent = new EventEmitter<AdminCinema>();

  cinemaForm: FormGroup = null!;

  ngOnInit() {
    this.cinemaForm = new FormGroup({
      cinemaNameControl: new FormControl(this.oldCinema.cinemaName, [
        Validators.required,
      ]),
      addressControl: new FormControl(this.oldCinema.cinemaAddress, [
        Validators.required,
      ]),
    });
  }

  errorMessage = '';

  get cinemaName() {
    return this.cinemaForm.controls['cinemaNameControl'];
  }

  get address() {
    return this.cinemaForm.controls['addressControl'];
  }

  close() {
    this.closeEvent.emit(true);
  }

  onSubmit() {
    const cinemaName = this.cinemaForm.get('cinemaNameControl')?.value!;
    const address = this.cinemaForm.get('addressControl')?.value!;

    if (this.oldCinema.cinemaId != 0) {
      this.editEvent.emit({
        cinemaId: this.oldCinema.cinemaId,
        cinemaName: cinemaName,
        cinemaAddress: address,
      });
    } else {
      this.saveEvent.emit({
        cinemaName: cinemaName,
        address: address,
      });
    }

    this.close();
  }
}
