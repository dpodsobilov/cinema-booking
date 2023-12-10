import { Component, EventEmitter, Output } from '@angular/core';
import { AdminGenreCreation } from '../../../services/admin/admin-film.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { logBuilderStatusWarnings } from '@angular-devkit/build-angular/src/builders/browser-esbuild/builder-status-warnings';

@Component({
  selector: 'app-genre-modal',
  templateUrl: './genre-modal.component.html',
  styleUrls: ['./genre-modal.component.css'],
})
export class GenreModalComponent {
  genreName: string = '';
  @Output() closeEvent = new EventEmitter<boolean>();
  @Output() saveEvent = new EventEmitter<AdminGenreCreation>();

  genreForm = new FormGroup({
    genreNameControl: new FormControl('', [Validators.required]),
  });

  errorMessage = '';

  get name() {
    return this.genreForm.controls['genreNameControl'];
  }

  close() {
    this.closeEvent.emit(true);
  }

  onSubmit() {
    this.genreName = this.genreForm.get('genreNameControl')?.value!;
    this.saveEvent.emit({
      name: this.genreName,
    });
    this.close();
  }
}
