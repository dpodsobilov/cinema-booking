namespace Data.Models;

public class Genre
{
    public int GenreId { get; set; }
    public string? GenreName { get; set; } = null!;

    // GenreId -> FK в FilmGenre
    public ICollection<FilmGenre> FilmGenres { get; set; } = null!;
}