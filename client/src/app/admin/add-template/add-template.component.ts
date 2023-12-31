import { Component, OnInit } from '@angular/core';
import {
  AddTemplateService,
  Places,
  PlacesTypes,
  ResponseMatrix,
} from '../../services/admin/add-template.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomError } from '../../services/admin/admin-cinemas.service';

@Component({
  selector: 'app-add-template',
  templateUrl: './add-template.component.html',
  styleUrls: ['./add-template.component.css'],
})
export class AddTemplateComponent implements OnInit {
  templateName: string = '';
  templateId: number = 0;
  newColor: string = '';
  matrix: Places[][] = [[]];
  placesTypes: PlacesTypes[] = [];
  selectedType: number = 0;
  sendMatrix: number[][] = [[]];

  constructor(
    private addTemplatesService: AddTemplateService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.route.queryParams.subscribe((params) => {
      this.templateId = params['templateId'];
      this.templateName = params['templateName'];
    });
  }
  ngOnInit(): void {
    this.addTemplatesService
      .getPlacesTypes()
      .subscribe((res: PlacesTypes[]) => {
        this.placesTypes = res;
        this.selectedType = this.placesTypes[0].placeTypeId;
      });

    //Если добавляем новый, то матрица пустая
    if (this.templateId === undefined) {
      for (let i = 0; i < 10; i++) {
        this.matrix[i] = [];
        for (let j = 0; j < 15; j++) {
          this.matrix[i][j] = { placeTypeId: 0, color: 'lightgrey' };
        }
      }
    }
    //Если редактируем, то получаем текущий с сервера
    else {
      this.addTemplatesService
        .getTemplateMatrix(this.templateId)
        .subscribe((res: ResponseMatrix) => {
          this.matrix = res.adminPlaceDtos;
        });
    }
    //инициализации матрицы для отправки
    for (let i = 0; i < 10; i++) {
      this.sendMatrix[i] = [];
      for (let j = 0; j < 15; j++) {
        this.sendMatrix[i][j] = 0;
      }
    }
  }

  onPlaceSelect(row: number, column: number) {
    if (this.matrix[row][column].placeTypeId === this.selectedType) {
      this.matrix[row][column].placeTypeId = 0;
      this.matrix[row][column].color = 'lightgrey';
    } else {
      for (let i = 0; i < this.placesTypes.length; i++) {
        if (this.placesTypes[i].placeTypeId === this.selectedType) {
          this.newColor = this.placesTypes[i].placeTypeColor;
          break;
        }
      }
      this.matrix[row][column].placeTypeId = this.selectedType;
      this.matrix[row][column].color = this.newColor;
    }
  }

  reset() {
    if (confirm('Вы уверены что хотите очистить шаблон?')) {
      for (let i = 0; i < 10; i++) {
        this.matrix[i] = [];
        for (let j = 0; j < 15; j++) {
          this.matrix[i][j] = { placeTypeId: 0, color: 'lightgrey' };
        }
      }
      this.templateName = '';
    }
  }
  save() {
    if (this.templateName !== '') {
      if (confirm('Вы уверены что хотите сохранить изменения?')) {
        for (let i = 0; i < this.matrix.length; i++) {
          for (let j = 0; j < this.matrix[i].length; j++) {
            this.sendMatrix[i][j] = this.matrix[i][j].placeTypeId;
          }
        }
        if (this.templateId == undefined) {
          this.addTemplatesService.sendMatr.CinemaHallTypeName =
            this.templateName;
          this.addTemplatesService.sendMatr.TemplatePlaceTypes =
            this.sendMatrix;
          this.addTemplatesService.createMatrix().subscribe({
            next: (response) => {
              this.router.navigate(['/admin/templates/']);
            },
            error: (e: CustomError) => {
              alert('Ошибка: ' + e.error.Message);
            },
          });
        } else {
          this.addTemplatesService.updMatr.CinemaHallTypeId = this.templateId;
          this.addTemplatesService.updMatr.CinemaHallTypeName =
            this.templateName;
          this.addTemplatesService.updMatr.TemplatePlaceTypes = this.sendMatrix;

          this.addTemplatesService.updateMatrix().subscribe({
            next: (response) => {
              this.router.navigate(['/admin/templates/']);
            },
            error: (e: CustomError) => {
              alert('Ошибка: ' + e.error.Message);
            },
          });
        }
      }
    } else alert('Введите название шаблона!');
  }
}
