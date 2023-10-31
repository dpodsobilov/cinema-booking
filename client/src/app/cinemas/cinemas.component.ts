import {AfterViewInit, Component, DoCheck, Inject, OnInit} from '@angular/core';
import {HomepageService, CinemaFilms, HomePageFilm} from "../services/homepage-service/homepage.service";
import {FilmService} from "../services/film-service/film.service";

@Component({
    selector: 'app-cinemas',
    templateUrl: './cinemas.component.html',
    styleUrls: ['./cinemas.component.css']
})
export class CinemasComponent implements OnInit{
  cinemaFilms:CinemaFilms[] = []
  films:HomePageFilm[] = []
  _cinemaFilms:CinemaFilms[] = []
  temp:number[]=[]
  SelectedCinema = localStorage.getItem('CinemaID')
  constructor(private homepageService: HomepageService/*private filmService: FilmService*/) {
  }

  ngOnInit(): void {
    this.homepageService.GetHomePageFilms().subscribe((res:CinemaFilms[]) => {
      this.cinemaFilms = res;
      this.addAllFilms()
    })
  }


  addAllFilms(){
    this.films = []
    for(let i = 0; i < this.cinemaFilms.length; i++ ){
      for(let j = 0; j < this.cinemaFilms[i].films.length; j++ ){
        if(this.films.find(f => f.filmId === this.cinemaFilms[i].films[j].filmId) === undefined){
          this.films.push(this.cinemaFilms[i].films[j])
        }
      }
    }
  }

  oncinemaSelected(id: number){
    let counter = 1

    for(let i = 0; i<this.temp.length; i++){
      if(this.temp.includes(id)){
        counter++
      }
      else{this.temp = []}
    }

    this.temp.push(id)

    if (counter%2!==0){
      for(let i = 0; i < this.cinemaFilms.length; i++ ){
        if(id === this.cinemaFilms[i].cinemaId){
          this.films = this.cinemaFilms[i].films
        }
        (document.getElementById(String(this.cinemaFilms[i].cinemaId)) as HTMLElement).style.backgroundColor = "transparent"
      }
      (document.getElementById(String(id)) as HTMLElement).style.backgroundColor = "#1DE782"
    }
    else{
      (document.getElementById(String(id)) as HTMLElement).style.backgroundColor = "transparent"
      this.addAllFilms()
    }
  }
}
