namespace Data.Models;

public class FilmGenre
{
    public int FilmGenreId { get; set; }
    //FK - FilmId
    public int FilmId { get; set; }
    public Film Film { get; set; } = null!;
    //FK - GenreId
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
}