import { AfterContentInit, Component } from '@angular/core';
import { SystemInfoComponent } from '../system-info.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-instruction',
  templateUrl: './instruction.component.html',
  styleUrls: ['./instruction.component.css'],
})
export class InstructionComponent {
  private fragment: string = '';
  constructor(
    private sys: SystemInfoComponent,
    private route: ActivatedRoute,
  ) {
    // this.sys.isDevInfo = false
  }

  // ngOnInit() {
  //   this.route.fragment.subscribe(fragment => { this.fragment = fragment!; });
  // }
  //
  // ngAfterViewInit(): void {
  //   try {
  //     document.querySelector('#' + this.fragment).scrollIntoView();
  //   } catch (e) { }
  // }
}
