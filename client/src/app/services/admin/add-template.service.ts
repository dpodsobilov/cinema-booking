import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Film } from '../film.service';

export interface PlacesTypes {
  placeTypeId: number;
  placeTypeName: string;
  placeTypeColor: string;
}
export interface Places {
  placeTypeId: number;
  color: string;
}
export interface SendMatrix {
  CinemaHallTypeName: string;
  TemplatePlaceTypes: number[][];
}

export interface UpdateMatrix {
  CinemaHallTypeId: number;
  CinemaHallTypeName: string;
  TemplatePlaceTypes: number[][];
}

export interface ResponseMatrix {
  adminPlaceDtos: Places[][];
}
@Injectable({
  providedIn: 'root',
})
export class AddTemplateService {
  sendMatr: SendMatrix = { CinemaHallTypeName: '', TemplatePlaceTypes: [] };
  updMatr: UpdateMatrix = {
    CinemaHallTypeId: 0,
    CinemaHallTypeName: '',
    TemplatePlaceTypes: [],
  };

  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getPlacesTypes(): Observable<PlacesTypes[]> {
    return this.http.get<PlacesTypes[]>(this.baseUrl + '/Admin/PlacesTypes');
  }

  createMatrix() {
    return this.http.post(this.baseUrl + '/Admin/Template', this.sendMatr, {
      observe: 'response',
    });
  }

  updateMatrix() {
    return this.http.put(this.baseUrl + '/Admin/Template', this.updMatr, {
      observe: 'response',
    });
  }

  getTemplateMatrix(templateId: number): Observable<ResponseMatrix> {
    return this.http.get<ResponseMatrix>(
      this.baseUrl + '/Admin/Template?' + 'param=' + templateId,
    );
  }
}
