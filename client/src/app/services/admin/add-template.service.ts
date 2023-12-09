import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PlacesTypes {
  placeTypeId: number;
  placeTypeName: string;
  placeTypeColor: string;
}

export interface Places {
  placeTypeId: number;
  color: string;
}
@Injectable({
  providedIn: 'root',
})
export class AddTemplateService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getPlacesTypes(): Observable<PlacesTypes[]> {
    return this.http.get<PlacesTypes[]>(this.baseUrl + '/Admin/PlacesTypes');
  }
}
