import { Component } from '@angular/core';
import {
  AdminTemplates,
  AdminTemplateService,
} from '../../services/admin/admin-template.service';
import { CustomError } from '../../services/admin/admin-cinemas.service';

@Component({
  selector: 'app-templates',
  templateUrl: './templates.component.html',
  styleUrls: ['./templates.component.css'],
})
export class TemplatesComponent {
  templates: AdminTemplates[] = [];
  constructor(private adminTemplateService: AdminTemplateService) {}

  ngOnInit() {
    this.adminTemplateService
      .getTemplates()
      .subscribe((res: AdminTemplates[]) => {
        this.templates = res;
      });
  }

  deleteTemplate(templateId: number) {
    this.adminTemplateService.deleteTemplate(templateId).subscribe({
      next: (response) => {
        this.adminTemplateService
          .getTemplates()
          .subscribe((res: AdminTemplates[]) => {
            this.templates = res;
          });
      },
      error: (e: CustomError) => {
        alert('Ошибка: ' + e.error.Message);
      },
    });
  }
}
