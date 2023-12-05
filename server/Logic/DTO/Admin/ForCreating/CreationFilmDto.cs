namespace Logic.DTO.Admin.ForCreating;

public class CreationFilmDto
{
    public string FilmName { get; set; } = null!;
    public string Hours { get; set; } = null!;
    public string Minutes { get; set; } = null!;
    public decimal FilmCoefficient { get; set; }
    public string Description { get; set; } = null!;
    public int Year { get; set; }
    public string Poster { get; set; } = null!;
    public int[] Genres { get; set; } = null!;
}