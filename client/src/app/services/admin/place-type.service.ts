import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminTemplates } from './admin-template.service';
import { AdminGenre, AdminGenreCreation } from './admin-film.service';

export interface PlaceType {
  typeId: number;
  name: string;
  color: string;
  cost: number;
}

export interface AdminPlaceTypeCreation {
  name: string;
  color: string;
  cost: number;
}

@Injectable({
  providedIn: 'root',
})
export class PlaceTypeService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getTypes(): Observable<PlaceType[]> {
    return this.http.get<PlaceType[]>(this.baseUrl + '/Admin/PlaceType');
  }

  addType(placeType: AdminPlaceTypeCreation) {
    return this.http.post(this.baseUrl + '/Admin/PlaceType?', placeType, {
      observe: 'response',
    });
  }

  editType(placeType: PlaceType) {
    return this.http.put(this.baseUrl + '/Admin/PlaceType?', placeType, {
      observe: 'response',
    });
  }

  deleteType(placeTypeId: number) {
    return this.http.delete(
      this.baseUrl + '/Admin/PlaceType?' + 'placeTypeId=' + placeTypeId,
      {
        observe: 'response',
      },
    );
  }
}
