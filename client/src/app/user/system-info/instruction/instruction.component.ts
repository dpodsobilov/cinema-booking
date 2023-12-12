import { AfterContentInit, Component } from '@angular/core';
import { SystemInfoComponent } from '../system-info.component';

@Component({
  selector: 'app-instruction',
  templateUrl: './instruction.component.html',
  styleUrls: ['./instruction.component.css'],
})
export class InstructionComponent {
  constructor(private sys: SystemInfoComponent) {
    // this.sys.isDevInfo = false
  }
}
