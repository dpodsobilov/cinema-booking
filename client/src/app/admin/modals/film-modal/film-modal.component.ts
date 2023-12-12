import { Component, EventEmitter, Input, Output } from '@angular/core';
import {
  AdminFilmCreation,
  AdminFilmEditing,
  AdminFilmService,
  AdminGenre,
} from '../../../services/admin/admin-film.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-film-modal',
  templateUrl: './film-modal.component.html',
  styleUrls: ['./film-modal.component.css'],
})
export class FilmModalComponent {
  @Input() isEditing = false;
  @Input() oldFilm: AdminFilmEditing = {
    filmId: 0,
    filmName: '',
    hours: '',
    minutes: '',
    filmCoefficient: 0,
    description: '',
    year: 0,
    poster: '',
    genres: [],
  };
  @Output() closeEvent = new EventEmitter<boolean>();
  @Output() saveEvent = new EventEmitter<AdminFilmCreation>();
  @Output() editEvent = new EventEmitter<AdminFilmEditing>();
  genres: AdminGenre[] = [];
  filmForm: FormGroup = null!;
  currentYear: number = new Date().getFullYear();
  image: string = '';

  constructor(private adminFilmService: AdminFilmService) {}

  ngOnInit() {
    this.adminFilmService.getGenres().subscribe({
      next: (response) => {
        this.genres = response;
      },
      error: (e) => {
        alert('Не удалось загрузить жанры!');
      },
    });

    this.filmForm = new FormGroup({
      nameControl: new FormControl(this.oldFilm.filmName, [
        Validators.required,
      ]),
      descriptionControl: new FormControl(this.oldFilm.description, [
        Validators.required,
      ]),
      hoursControl: new FormControl(+this.oldFilm.hours, [
        Validators.required,
        Validators.min(0),
        Validators.max(3),
      ]),
      minutesControl: new FormControl(+this.oldFilm.minutes, [
        Validators.required,
        Validators.min(0),
        Validators.max(59),
      ]),
      coefficientControl: new FormControl(this.oldFilm.filmCoefficient, [
        Validators.required,
        Validators.min(1),
      ]),
      yearControl: new FormControl(this.oldFilm.year, [
        Validators.required,
        Validators.min(1900),
        Validators.max(new Date().getFullYear()),
      ]),
      genreControl: new FormControl(this.oldFilm.genres, [Validators.required]),
      posterControl: new FormControl(this.oldFilm.poster, [
        Validators.required,
      ]),
    });
  }

  errorMessage = '';

  get name() {
    return this.filmForm.controls['nameControl'];
  }
  get description() {
    return this.filmForm.controls['descriptionControl'];
  }
  get hours() {
    return this.filmForm.controls['hoursControl'];
  }
  get minutes() {
    return this.filmForm.controls['minutesControl'];
  }
  get coefficient() {
    return this.filmForm.controls['coefficientControl'];
  }
  get year() {
    return this.filmForm.controls['yearControl'];
  }
  get genre() {
    return this.filmForm.controls['genreControl'];
  }
  get poster() {
    return this.filmForm.controls['posterControl'];
  }

  changeListener(event: Event): void {
    this.readThis(event.target);
  }

  readThis(inputValue: any): void {
    let file: File = inputValue.files[0];
    let myReader: FileReader = new FileReader();

    myReader.onloadend = (e) => {
      this.image = myReader.result!.toString().split(',')[1];
      console.log(this.image);
    };
    myReader.readAsDataURL(file);
  }

  close() {
    this.closeEvent.emit(true);
  }

  onSubmit() {
    const name = this.filmForm.get('nameControl')?.value!;
    const description = this.filmForm.get('descriptionControl')?.value!;
    const hours = this.filmForm.get('hoursControl')?.value!.toString();
    const minutes = this.filmForm.get('minutesControl')?.value!.toString();
    const coefficient = +this.filmForm.get('coefficientControl')?.value!;
    const year = +this.filmForm.get('yearControl')?.value!;
    const genre = this.filmForm.get('genreControl')?.value!;

    if (this.oldFilm.filmId != 0) {
      this.editEvent.emit({
        filmId: this.oldFilm.filmId,
        filmName: name,
        description: description,
        hours: hours,
        minutes: minutes,
        filmCoefficient: coefficient,
        year: year,
        genres: genre,
        poster: this.image,
      });
    } else {
      this.saveEvent.emit({
        filmName: name,
        description: description,
        hours: hours,
        minutes: minutes,
        filmCoefficient: coefficient,
        year: year,
        genres: genre,
        poster: this.image,
      });
    }
    this.close();
  }
}
