import {Component, OnInit} from '@angular/core';
import {Film, FilmService, Schedule} from "../services/film-service/film.service";
import { ActivatedRoute } from '@angular/router';

export interface Cinema{
  cinemaId: number
  cinemaName: string
}
export interface CinemaHall{
  cinemaHallId: number
  cinemaHallName: string
}
export interface DayMonth{
  date: Date
  dateValue: string
}

@Component({
  selector: 'app-film',
  templateUrl: './film.component.html',
  styleUrls: ['./film.component.css']
})
export class FilmComponent implements OnInit{
  cinemas: Cinema[] = []
  cinemaHalls: CinemaHall[] = []
  datesStr:string[] = []
  dates:Date[] = []
  dayMonths:DayMonth[] = []
  times:DayMonth[] = []
  selectedCinema: number = 0
  selectedHall: number = 0
  selectedDate:Date = new Date('00-00-00')
  film:Film = {filmId: 0, filmName: '', duration: '', description: '', poster: '', filmGenres: []}
  schedule:Schedule[] = []
  segmentValue:string=''
  tempCinema:number[]=[]
  tempHall:number[]=[]
  tempDate:string[]=[]
  counterDate:number=0

  constructor(private filmService: FilmService, private route: ActivatedRoute) {
  }
  ngOnInit(): void {
    this.route.url.subscribe(segments => {
        this.segmentValue = segments[segments.length - 1].path;
    });


    this.filmService.GetFilmInfo(Number(this.segmentValue)).subscribe((res:Film) => {
      this.film = res;
      this.filmService.GetSchedule(Number(this.segmentValue)).subscribe((res:Schedule[]) => {
        this.schedule = res;
          this.addCinema()
      })
    })
  }
  addCinema(){

    //Тут еще должна быть проверка на localstorage и если там уже есть выбранный кинотеатр то
    //надо вызвать oncinemaSelected с этим параметром

    this.cinemas=[]
    for(let i = 0; i < this.schedule.length; i++){
      if(this.cinemas.find(c => c.cinemaName === this.schedule[i].cinemaName) === undefined){
        this.cinemas.push({
          cinemaId: this.schedule[i].cinemaId,
          cinemaName: this.schedule[i].cinemaName
        })
      }
    }
  }

  convertDate(){
    this.dates = []
      for(let i = 0; i<this.datesStr.length; i++){
        this.dates.push(new Date(Date.parse(this.datesStr[i])))
      }
  }
  getDayMonth(){
    this.dayMonths = []
    for(let i = 0; i<this.dates.length; i++){
        if(this.dayMonths.find(dm => this.removeTime(dm.date) === this.removeTime(this.dates[i])) === undefined) {
          this.dayMonths.push({
              date: this.dates[i],
              dateValue: (this.dates[i].getDate() + ' ' + this.parseMonth(this.dates[i].getMonth()))
          })
        }
    }
  }

  removeTime(date: Date) {
      return (
          (date.getFullYear()).toString() +
          (date.getMonth()).toString() +
          (date.getDate()).toString()
      );
  }

  parseMonth(month:number): string{
      let months:{[index:string]:string} = {
          "0": "Янв",
          "1": "Фев",
          "2": "Мар",
          "3": "Апр",
          "4": "Май",
          "5": "Июн",
          "6": "Июл",
          "7": "Авг",
          "8": "Сен",
          "9": "Окт",
          "10": "Ноя",
          "11": "Дек",
      }
    return months[month.toString()]
  }

  getTime(date:string){
    let converter = new Date(Date.parse(date))
      return (
          (converter.getHours()).toString() +
              ':' +
          (converter.getMinutes()).toString()
      );
  }

  oncinemaSelected(cinema: Cinema){
    this.datesStr = []
    this.cinemaHalls = []
    this.times = []
    let counter = 1

    for(let i = 0; i<this.tempCinema.length; i++){
      if(this.tempCinema.includes(cinema.cinemaId)){
        counter++
      }
      else{this.tempCinema = []}
    }

    this.tempCinema.push(cinema.cinemaId)

    if (counter%2!==0){ //если выбран элемент
      for(let i = 0; i < this.schedule.length; i++ ){
          //Если выбран кинотеатр то заполняем кинозалы
        if(cinema.cinemaId === this.schedule[i].cinemaId){
          //Проврека на повторы кинозалов
          if(this.cinemaHalls.find(ch => ch.cinemaHallName === this.schedule[i].cinemaHallName) === undefined){
            this.cinemaHalls.push({
                cinemaHallId: this.schedule[i].cinemaHallId,
                cinemaHallName: this.schedule[i].cinemaHallName
            })
          }
            this.selectedCinema = cinema.cinemaId
        }
      }
      //Окрашиваем кнопку кинотеатра
      for(let i = 0; i < this.cinemas.length; i++ ){
        (document.getElementById('cinema-' + String(this.cinemas[i].cinemaId)) as HTMLElement).style.backgroundColor = "transparent"
      }
      (document.getElementById('cinema-' + String(cinema.cinemaId)) as HTMLElement).style.backgroundColor = "#1DE782"
      // Тут выводим дни когда есть сеансы в кинотеатре
      for(let i = 0; i < this.schedule.length; i++ ){
        if(this.selectedCinema === this.schedule[i].cinemaId){
          if(!this.datesStr.includes(this.schedule[i].dataTimeSession)){
              this.datesStr.push(this.schedule[i].dataTimeSession)
          }
        }
      }
      this.convertDate()
      this.getDayMonth()

    }
    else{ //Если ничего не выбрано
      (document.getElementById('cinema-' + String(cinema.cinemaId)) as HTMLElement).style.backgroundColor = "transparent"
      this.selectedCinema = 0
      this.datesStr = []
      this.dayMonths = []
      this.times = []
    }
  }

  onCinemaHallSelected(hall: CinemaHall){
    this.datesStr = []
    this.times = []
    let counter = 1
    this.tempDate = []

    for(let i = 0; i<this.tempHall.length; i++){
      if(this.tempHall.includes(hall.cinemaHallId)){
        counter++
      }
      else{this.tempHall = []}
    }

    this.tempHall.push(hall.cinemaHallId)
    if (counter%2!==0){ //если выбран элемент
      //Если выбран кинозал то заполняем даты
      for(let i = 0; i < this.schedule.length; i++ ){
        if(hall.cinemaHallId === this.schedule[i].cinemaHallId){
          //Проврека на повторы дат
          if(!this.datesStr.includes(this.schedule[i].dataTimeSession)){
            this.datesStr.push(this.schedule[i].dataTimeSession)
          }
        }
      }
      this.convertDate()
      this.getDayMonth()
      this.selectedHall = hall.cinemaHallId
      //Окрашиваем кнопку кинозала
      for(let i = 0; i < this.cinemaHalls.length; i++ ){
        (document.getElementById('hall-'+ String(this.cinemaHalls[i].cinemaHallId)) as HTMLElement).style.backgroundColor = "transparent"
      }
      (document.getElementById('hall-'+ String(hall.cinemaHallId)) as HTMLElement).style.backgroundColor = "#1DE782"

    }
    else{ //Если ничего не выбрано
      (document.getElementById('hall-'+ String(hall.cinemaHallId)) as HTMLElement).style.backgroundColor = "transparent"

      this.times = []
      this.selectedHall = 0
      //Заполняем даты по выбранному кинотеатру
      for(let i = 0; i < this.schedule.length; i++ ){
          if(this.selectedCinema === this.schedule[i].cinemaId){
              //Проврека на повторы дат
              if(!this.datesStr.includes(this.schedule[i].dataTimeSession)){
                  this.datesStr.push(this.schedule[i].dataTimeSession)
              }
          }
      }
      this.convertDate()
      this.getDayMonth()
    }
  }

  onDateSelected(date:DayMonth){
      this.times = []
      this.counterDate = 1
      for(let i = 0; i<this.tempDate.length; i++){
          if(this.tempDate.includes(date.dateValue)){
            this.counterDate++
          }
          else{this.tempDate = []}
      }

      this.tempDate.push(date.dateValue)

      if (this.counterDate%2!==0){ //если выбран элемент
        //Если выбран день то заполняем время
        for(let i = 0; i < this.schedule.length; i++ ){
          if(this.removeTime(date.date) === this.removeTime(new Date(Date.parse(this.schedule[i].dataTimeSession)))){
            //Если выбран кинозал заполняем время по нему
            if((this.selectedHall !== 0) && (this.selectedHall === this.schedule[i].cinemaHallId)){
              if(this.times.find(t => t.date === (new Date(Date.parse(this.schedule[i].dataTimeSession)))) === undefined){
                this.times.push({
                    date: (new Date(Date.parse(this.schedule[i].dataTimeSession))),
                    dateValue: this.getTime(this.schedule[i].dataTimeSession)
                })
              }
            }
            //Если кинозал не выбран, заполняем время по кинотеатру
            if((this.selectedHall === 0) && (this.selectedCinema === this.schedule[i].cinemaId)){
                if(this.times.find(t => t.date === (new Date(Date.parse(this.schedule[i].dataTimeSession)))) === undefined){
                    this.times.push({
                        date: (new Date(Date.parse(this.schedule[i].dataTimeSession))),
                        dateValue: this.getTime(this.schedule[i].dataTimeSession)
                    })
                }
            }
          }
        }
        this.selectedDate = date.date
        //Окрашиваем кнопку дня
        for(let i = 0; i < this.dayMonths.length; i++ ){
          (document.getElementById('date-' + String(this.dayMonths[i].date)) as HTMLElement).style.backgroundColor = "transparent"
        }
        (document.getElementById('date-' + String(date.date)) as HTMLElement).style.backgroundColor = "#1DE782"
      }
      else{ //Если ничего не выбрано
          (document.getElementById('date-' + String(date.date)) as HTMLElement).style.backgroundColor = "transparent"
          this.times = []
          this.selectedDate = new Date('00-00-00')
      }
  }
}
