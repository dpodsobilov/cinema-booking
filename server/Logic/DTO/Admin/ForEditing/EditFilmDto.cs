namespace Logic.DTO.Admin.ForEditing;

public class EditFilmDto
{
    public int FilmId { get; set; }
    public string FilmName { get; set; } = null!;
    public string Hours { get; set; } = null!;
    public string Minutes { get; set; } = null!;
    public decimal FilmCoefficient { get; set; }
    public string Description { get; set; } = null!;
    public int Year { get; set; }
    public byte[] Poster { get; set; } = null!;
    public int[] Genres { get; set; } = null!;
}
