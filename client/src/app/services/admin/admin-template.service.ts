import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AdminTemplates {
  templateId: number;
  templateName: string;
}
@Injectable({
  providedIn: 'root',
})
export class AdminTemplateService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getTemplates(): Observable<AdminTemplates[]> {
    return this.http.get<AdminTemplates[]>(this.baseUrl + '/Admin/Templates');
  }

  deleteTemplate(templateId: number) {
    return this.http.delete(
      this.baseUrl + '/Admin/Template?' + 'templateId=' + templateId,
      {
        observe: 'response',
      },
    );
  }
}
