import { Component, OnInit } from '@angular/core';
import {
  AdminUsers,
  AdminUsersService,
} from '../../services/admin/admin-users.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
})
export class UsersComponent implements OnInit {
  users: AdminUsers[] = [];
  constructor(private adminUsersService: AdminUsersService) {}

  ngOnInit(): void {
    this.adminUsersService.getUsers().subscribe((res: AdminUsers[]) => {
      this.users = res;
    });
  }
}
