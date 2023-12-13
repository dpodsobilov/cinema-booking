namespace Logic.DTO.User;

public class FilmDto
{
    public int FilmId { get; set; }
    public string FilmName { get; set; } = null!;
    public string Duration { get; set; } = null!;
    public string Description { get; set; } = null!;
    public byte[] Poster { get; set; } = null!;
    
    // FilmId -> FK в FilmGenre
    public List<string> FilmGenres { get; set; } = null!;
}