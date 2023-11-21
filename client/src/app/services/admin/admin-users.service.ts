import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

export interface AdminUsers {
  email: string;
  name: string;
  surname: string;
}
@Injectable({
  providedIn: 'root',
})
export class AdminUsersService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_API_URL') private baseUrl: string,
  ) {}

  getUsers(): Observable<AdminUsers[]> {
    return this.http.get<AdminUsers[]>(this.baseUrl + '/Admin/Users');
  }
}
