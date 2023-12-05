using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Film
{
    public int FilmId { get; set; }
    public string FilmName { get; set; } = null!;
    public string Duration { get; set; } = null!;
    public decimal FilmCoefficient { get; set; }
    public string Description { get; set; } = null!;
    public byte[] Poster { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    // FilmId -> FK в FilmGenre
    public ICollection<FilmGenre> FilmGenres { get; set; } = null!;
}