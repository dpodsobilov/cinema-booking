import {
  AfterContentInit,
  Component,
  ElementRef,
  Input,
  ViewChild,
} from '@angular/core';
import { Ticket } from '../../services/ticket.service';
import { jsPDF } from 'jspdf';
import { font } from 'src/assets/Montserrat-Regular-bold';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css'],
})
export class TicketComponent implements AfterContentInit {
  @ViewChild('canvasElement', { static: false })
  canvas!: ElementRef<HTMLCanvasElement>;
  @ViewChild('imageElement', { static: false })
  canvasImg!: ElementRef<HTMLCanvasElement>;
  @Input('ticket') ticket: Ticket = {
    ticketId: 0,
    date: '',
    cinemaHallName: '',
    cinemaName: '',
    filmName: '',
    placeName: '',
    placeTypeName: '',
  };
  @Input('isUpcoming') isUpcoming = false;

  qrCodeData: string = '';
  ngAfterContentInit() {
    this.qrCodeData =
      'Билет \nДата: ' +
      this.getDate(this.ticket.date) +
      '\nФильм: ' +
      this.ticket.filmName +
      '\nКинотеатр: ' +
      this.ticket.cinemaName +
      '\nКинозал: ' +
      this.ticket.cinemaHallName +
      '\nТип места: ' +
      this.ticket.placeTypeName +
      '\nМесто: ' +
      this.ticket.placeName +
      '\nВремя: ' +
      this.getTime(this.ticket.date);
  }

  getDate(date: string) {
    let converter = new Date(Date.parse(date));
    return (
      this.parseDayOfWeek(converter.getDay()) +
      ', ' +
      converter.getDate().toString() +
      ' ' +
      this.parseMonth(converter.getMonth()) +
      ' ' +
      converter.getFullYear().toString()
    );
  }
  getTime(date: string) {
    let converter = new Date(Date.parse(date));
    return (
      converter.getHours().toString() + ':' + converter.getMinutes().toString()
    );
  }
  parseMonth(month: number): string {
    let months: { [index: string]: string } = {
      '0': 'Янв',
      '1': 'Фев',
      '2': 'Мар',
      '3': 'Апр',
      '4': 'Май',
      '5': 'Июн',
      '6': 'Июл',
      '7': 'Авг',
      '8': 'Сен',
      '9': 'Окт',
      '10': 'Ноя',
      '11': 'Дек',
    };
    return months[month.toString()];
  }

  parseDayOfWeek(day: number): string {
    let days: { [index: string]: string } = {
      '0': 'Вс',
      '1': 'Пн',
      '2': 'Вт',
      '3': 'Ср',
      '4': 'Чт',
      '5': 'Пт',
      '6': 'Сб',
    };
    return days[day.toString()];
  }

  downloadTicket() {
    //создаем pdf
    const doc = new jsPDF({ orientation: 'l', format: [85, 145] });
    //устанавливаем фон билета - получаем блок с картинкой
    const canvasImg = this.canvasImg.nativeElement;
    //получаем картинку из этого блока
    const background = canvasImg.querySelector('img')!;
    if (background) {
      doc.addImage(
        background,
        'JPEG',
        0,
        0,
        doc.internal.pageSize.width,
        doc.internal.pageSize.height,
      );
    }
    //добавление шрифтов
    doc.addFileToVFS('Montserrat-Regular-normal.ttf', font);
    doc.addFont('Montserrat-Regular-normal.ttf', 'Montserrat', 'normal');
    //подготовка данных
    const ticketData = {
      //данные о билете
      Date: this.getDate(this.ticket.date),
      Film: this.ticket.filmName.toUpperCase(),
      Cinema: this.ticket.cinemaName,
      CinemaHall: this.ticket.cinemaHallName,
      PlaceType: this.ticket.placeTypeName,
      Place: this.ticket.placeName,
      Time: this.getTime(this.ticket.date),
    };
    // Настройка стилей
    doc.setFontSize(12);
    doc.setFont('Montserrat');
    doc.setTextColor(255, 255, 255);
    // Создание содержимого PDF с настроенными стилями
    doc.text('Билет', 10, 10);
    doc.text(`Дата: ${ticketData.Date}`, 10, 20);
    doc.text(`Фильм: ${ticketData.Film}`, 10, 30);
    doc.text(`Кинотеатр: ${ticketData.Cinema}`, 10, 40);
    doc.text(`Кинозал: ${ticketData.CinemaHall}`, 10, 50);
    doc.text(`Тип места: ${ticketData.PlaceType}`, 10, 60);
    doc.text(`Место: ${ticketData.Place}`, 10, 70);
    doc.text(`Время: ${ticketData.Time}`, 10, 80);
    //получаем блок с qr кодом
    const canvas = this.canvas.nativeElement;
    //получаем картинку из этого блока
    const imgElement = canvas.querySelector('img')!;
    if (imgElement) {
      doc.addImage(imgElement, 'JPEG', 100, 41, 42, 42);
    }
    //сохраняем файл
    doc.save('ticket.pdf');
  }
}
