import { Component, EventEmitter, Input, Output } from '@angular/core';
import {
  AdminCinema,
  AdminCinemasService,
  AdminHall,
  AdminHallCreation,
} from '../../../services/admin/admin-cinemas.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  AdminTemplates,
  AdminTemplateService,
} from '../../../services/admin/admin-template.service';

@Component({
  selector: 'app-hall-modal',
  templateUrl: './hall-modal.component.html',
  styleUrls: ['./hall-modal.component.css'],
})
export class HallModalComponent {
  @Input() isEditing = false;
  @Input() oldHall: AdminHall = {
    cinemaHallId: 0,
    cinemaHallName: '',
    cinemaHallTypeName: '',
    cinemaId: 0,
  };
  @Output() closeEvent = new EventEmitter<boolean>();
  @Output() saveEvent = new EventEmitter<AdminHallCreation>();
  @Output() editEvent = new EventEmitter<AdminHall>();
  templates: AdminTemplates[] = [];
  cinemas: AdminCinema[] = [];

  hallForm: FormGroup = null!;

  constructor(
    public adminTemplateService: AdminTemplateService,
    public adminCinemasService: AdminCinemasService,
  ) {}

  ngOnInit() {
    this.adminTemplateService
      .getTemplates()
      .subscribe((res: AdminTemplates[]) => {
        this.templates = res;
      });

    this.adminCinemasService.getCinemas().subscribe((res: AdminCinema[]) => {
      this.cinemas = res;
    });

    this.hallForm = new FormGroup({
      hallName: new FormControl(this.oldHall.cinemaHallName, [
        Validators.required,
      ]),
      hallTypeName: new FormControl(this.oldHall.cinemaHallTypeName, [
        Validators.required,
      ]),
      cinemaId: new FormControl(this.oldHall.cinemaId, [Validators.required]),
    });
  }

  errorMessage = '';

  get hallName() {
    return this.hallForm.controls['hallName'];
  }

  get hallTypeName() {
    return this.hallForm.controls['hallTypeName'];
  }

  get cinemaId() {
    return this.hallForm.controls['cinemaId'];
  }

  close() {
    this.closeEvent.emit(true);
  }

  onSubmit() {
    const hallName = this.hallForm.get('hallName')?.value!;
    const hallTypeName = this.hallForm.get('hallTypeName')?.value!;
    const cinemaId = this.hallForm.get('cinemaId')?.value!;

    if (this.oldHall.cinemaHallId != 0) {
      this.editEvent.emit({
        cinemaHallId: this.oldHall.cinemaHallId,
        cinemaHallName: hallName,
        cinemaHallTypeName: hallTypeName,
        cinemaId: cinemaId,
      });
    } else {
      this.saveEvent.emit({
        cinemaHallName: hallName,
        cinemaHallTypeName: hallTypeName,
        cinemaId: cinemaId,
      });
    }

    this.close();
  }
}
