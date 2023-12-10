import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  AdminGenre,
  AdminGenreCreation,
} from '../../../services/admin/admin-film.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { logBuilderStatusWarnings } from '@angular-devkit/build-angular/src/builders/browser-esbuild/builder-status-warnings';

@Component({
  selector: 'app-genre-modal',
  templateUrl: './genre-modal.component.html',
  styleUrls: ['./genre-modal.component.css'],
})
export class GenreModalComponent implements OnInit {
  genreName: string = '';
  @Input() isEditing = false;
  @Input() oldGenre: AdminGenre = { genreId: null!, genreName: null! };
  @Output() closeEvent = new EventEmitter<boolean>();
  @Output() saveEvent = new EventEmitter<AdminGenreCreation>();
  @Output() editEvent = new EventEmitter<AdminGenre>();

  genreForm: FormGroup = null!;

  ngOnInit() {
    this.genreForm = new FormGroup({
      genreNameControl: new FormControl(this.oldGenre.genreName, [
        Validators.required,
      ]),
    });
  }

  errorMessage = '';

  get name() {
    return this.genreForm.controls['genreNameControl'];
  }

  close() {
    this.closeEvent.emit(true);
  }

  onSubmit() {
    this.genreName = this.genreForm.get('genreNameControl')?.value!;

    // если старое имя есть, значит мы редактируем
    if (this.oldGenre.genreId != undefined) {
      this.editEvent.emit({
        genreId: this.oldGenre.genreId,
        genreName: this.genreName,
      });
    } else {
      this.saveEvent.emit({
        name: this.genreName,
      });
    }

    this.close();
  }
}
