import { Component } from '@angular/core';
import {
  AdminTemplates,
  AdminTemplateService,
} from '../../services/admin/admin-template.service';

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
    this.adminTemplateService
      .deleteTemplate(templateId)
      .subscribe((response) => {
        if (response.status === 200) {
          this.adminTemplateService
            .getTemplates()
            .subscribe((res: AdminTemplates[]) => {
              this.templates = res;
            });
        } else alert('Ошибка! Удаление не выполнено!');
      });
  }
}
