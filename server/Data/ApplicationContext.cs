using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data;
public sealed class ApplicationContext : DbContext
{
    public DbSet<Cinema> Cinemas { get; set; } = null!;
    public DbSet<CinemaHall> CinemaHalls { get; set; } = null!;
    public DbSet<CinemaHallType> CinemaHallTypes { get; set; } = null!;
    public DbSet<Film> Films { get; set; } = null!;
    public DbSet<FilmGenre> FilmGenres { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Place> Places { get; set; } = null!;
    public DbSet<PlacePosition> PlacePositions { get; set; } = null!;
    public DbSet<PlaceType> PlaceTypes { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public ApplicationContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
        // if (Database.EnsureCreated())
        // { 
            User user1 = new User { Email = "daria@surf.ru", Password = "*", Name = "Daria", Surname = "Surf", Role = 0 };
            Users.Add(user1);
            byte[] img = FileConverter.GetBinaryFile("C:\\Users\\perep\\Pictures\\meme.jpg");
            Film film1 = new Film { FilmName = "Аватар", Duration = "1 час 10 минут", FilmCoefficient = 10, Description = "Синие человечки бьют друг друга волосами и сбивают мух, которые тоже вертолеты. В главных ролях: JSON стэтхэм, Коля Валуев, Ивалера Изгорилки", Poster = img};
            Film film2 = new Film { FilmName = "Не автар", Duration = "2 час 10 минут", FilmCoefficient = 10, Description = "Очень классный фильм", Poster = img};
            Film film3 = new Film { FilmName = "Аватар легенда об анге", Duration = "3 час 10 минут", FilmCoefficient = 10, Description = "Очень классный фильм", Poster = img};
            Film film4 = new Film { FilmName = "Коля Валуев: Бегущий по лезвию", Duration = "1 час 40 минут", FilmCoefficient = 10, Description = "Очень классный фильм", Poster = img};
            Film film5 = new Film { FilmName = "Космические медведи", Duration = "1 час 50 минут", FilmCoefficient = 10, Description = "Очень классный фильм", Poster = img};

            Films.Add(film1);
            Films.Add(film2);
            Films.Add(film3);
            Films.Add(film4);
            Films.Add(film5);

            Genre genre1 = new Genre { GenreName = "Комедия"};
            Genre genre2 = new Genre { GenreName = "Драма"};
            Genre genre3 = new Genre { GenreName = "Триллер"};
            Genre genre4 = new Genre { GenreName = "Ужасы"};
            Genre genre5 = new Genre { GenreName = "Мультфильм"};
            Genres.Add(genre1);
            Genres.Add(genre2);
            Genres.Add(genre3);
            Genres.Add(genre4);
            Genres.Add(genre5);

            Cinema cinema1 = new Cinema { CinemaName = "Zoom" , Address = "Tashkentskaya"};
            Cinema cinema2 = new Cinema { CinemaName = "Samara" , Address = "Dimitrova"};
            Cinemas.Add(cinema1);
            Cinemas.Add(cinema2);

            CinemaHallType cinemaHallType1 = new CinemaHallType { CinemaHallTypeName = "Мягкий"};
            CinemaHallTypes.Add(cinemaHallType1);

            PlaceType placeType1 = new PlaceType { PlaceName = "Обычное", DefaultCost = 200};
            PlaceTypes.Add(placeType1);

            PlacePosition placePosition1 = new PlacePosition { Row = 3, Number = 7};
            PlacePositions.Add(placePosition1);
            
            SaveChanges();

            CinemaHall cinemaHall1 = new CinemaHall {CinemaHallName = "Зал 7", CinemaHallTypeId = cinemaHallType1.CinemaHallTypeId, CinemaId = cinema1.CinemaId};
            CinemaHall cinemaHall2 = new CinemaHall {CinemaHallName = "Зал 666", CinemaHallTypeId = cinemaHallType1.CinemaHallTypeId, CinemaId = cinema1.CinemaId};
            CinemaHall cinemaHall3 = new CinemaHall {CinemaHallName = "Зал 999", CinemaHallTypeId = cinemaHallType1.CinemaHallTypeId, CinemaId = cinema2.CinemaId};
            
            CinemaHalls.Add(cinemaHall1);
            CinemaHalls.Add(cinemaHall2);
            CinemaHalls.Add(cinemaHall3);

            FilmGenre filmGenre1 = new FilmGenre { FilmId = film1.FilmId, GenreId = genre1.GenreId};
            FilmGenre filmGenre2 = new FilmGenre { FilmId = film2.FilmId, GenreId = genre2.GenreId};
            FilmGenre filmGenre3 = new FilmGenre { FilmId = film3.FilmId, GenreId = genre3.GenreId};
            FilmGenre filmGenre4 = new FilmGenre { FilmId = film4.FilmId, GenreId = genre4.GenreId};
            FilmGenre filmGenre5 = new FilmGenre { FilmId = film5.FilmId, GenreId = genre5.GenreId};
            FilmGenres.Add(filmGenre1);
            FilmGenres.Add(filmGenre2);
            FilmGenres.Add(filmGenre3);
            FilmGenres.Add(filmGenre4);
            FilmGenres.Add(filmGenre5);
            
            Place place1 = new Place { PlaceTypeId = placeType1.PlaceTypeId, PlacePositionId = placePosition1.PlacePositionId, CinemaHallTypeId = cinemaHallType1.CinemaHallTypeId};
            Places.Add(place1);
            
            SaveChanges();
            
            DateTime date = new DateTime(2023, 1, 20, 10, 30, 00);
            DateTime _date = new DateTime(2023, 1, 20, 15, 15, 00);
            DateTime date2 = new DateTime(2023, 2, 10, 18, 22, 00);
            DateTime date3 = new DateTime(2023, 3, 20, 10, 30, 00);
            // Session session1 = new Session { DataTimeSession = date, DataTimeCoefficient = 3, FilmId = film1.FilmId, CinemaHallId = cinemaHall1.CinemaHallId};
            Session session2 = new Session { DataTimeSession = date, DataTimeCoefficient = 3, FilmId = film3.FilmId, CinemaHallId = cinemaHall1.CinemaHallId};
            Session session3 = new Session { DataTimeSession = date, DataTimeCoefficient = 3, FilmId = film3.FilmId, CinemaHallId = cinemaHall2.CinemaHallId};
            Session session4 = new Session { DataTimeSession = date, DataTimeCoefficient = 3, FilmId = film4.FilmId, CinemaHallId = cinemaHall2.CinemaHallId};
            Session session5 = new Session { DataTimeSession = date, DataTimeCoefficient = 3, FilmId = film3.FilmId, CinemaHallId = cinemaHall3.CinemaHallId};
            Session session6 = new Session { DataTimeSession = date2, DataTimeCoefficient = 3, FilmId = film3.FilmId, CinemaHallId = cinemaHall1.CinemaHallId};
            Session session7 = new Session { DataTimeSession = _date, DataTimeCoefficient = 3, FilmId = film3.FilmId, CinemaHallId = cinemaHall1.CinemaHallId};


            // Sessions.Add(session1);
            Sessions.Add(session2);
            Sessions.Add(session3);
            Sessions.Add(session4);
            Sessions.Add(session5);
            Sessions.Add(session6);
            Sessions.Add(session7);

            
            
            SaveChanges();
            
            Ticket ticket1 = new Ticket { Price = 100, UserId = user1.UserId, SessionId = session2.SessionId, PlaceId = place1.PlaceId};
            Tickets.Add(ticket1);
            
            SaveChanges();   
        // }
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=PE_Project;Username=postgres;Password=123");
    }
}