import { Component, OnInit } from '@angular/core';
import { AdminCinemasService } from '../../services/admin/admin-cinemas.service';

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
}
