using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data;
public class ApplicationContext : DbContext
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
        
        User user1 = new User { Email = "daria@surf.ru", Password = "*", Name = "Daria", Surname = "Surf" };
        Users.Add(user1);
        byte[] img = File.ReadAllBytes("C:\\Users\\Ruslan\\Desktop\\ImgBlobConvert\\smile.bin");
        Film film1 = new Film { FilmName = "Avatar", Duration = "1 hour 10 minutes", FilmCoefficient = 10, Description = "Очень классный фильм", Poster = img};
        Films.Add(film1);

        Genre genre1 = new Genre { GenreName = "Comedy"};
        Genres.Add(genre1);

        Cinema cinema1 = new Cinema { CinemaName = "Zoom" , Address = "tashkentskaya"};
        Cinemas.Add(cinema1);

        CinemaHallType cinemaHallType1 = new CinemaHallType { CinemaHallTypeName = "Мягкий"};
        CinemaHallTypes.Add(cinemaHallType1);

        PlaceType placeType1 = new PlaceType { PlaceName = "Обычное", DefaultCost = 200};
        PlaceTypes.Add(placeType1);

        PlacePosition placePosition1 = new PlacePosition { Row = 3, Number = 7};
        PlacePositions.Add(placePosition1);
        
        SaveChanges();

        CinemaHall cinemaHall1 = new CinemaHall {CinemaHallName = "Зал 7", CinemaHallTypeId = cinemaHallType1.CinemaHallTypeId, CinemaId = cinema1.CinemaId};
        CinemaHalls.Add(cinemaHall1);

        FilmGenre filmGenre1 = new FilmGenre { FilmId = film1.FilmId, GenreId = genre1.GenreId};
        FilmGenres.Add(filmGenre1);
        
        Place place1 = new Place { PlaceTypeId = placeType1.PlaceTypeId, PlacePositionId = placePosition1.PlacePositionId, CinemaHallTypeId = cinemaHallType1.CinemaHallTypeId};
        Places.Add(place1);
        
        SaveChanges();
        
        DateTime date = new DateTime(2023, 1, 20, 10, 30, 00);
        Session session1 = new Session { DataTimeSession = date, DataTimeCoefficient = 3, FilmId = film1.FilmId, CinemaHallId = cinemaHall1.CinemaHallId};
        Sessions.Add(session1);
        
        SaveChanges();
        
        Ticket ticket1 = new Ticket { Price = 100, UserId = user1.UserId, SessionId = session1.SessionId, PlaceId = place1.PlaceId};
        Tickets.Add(ticket1);
        
        SaveChanges();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=PE_Project;Username=postgres;Password=Ruslan");
    }
}