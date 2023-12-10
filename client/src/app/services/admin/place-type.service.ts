import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminTemplates } from './admin-template.service';

export interface PlaceType {
  typeId: number;
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
}
