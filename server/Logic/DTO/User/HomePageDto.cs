namespace Logic.DTO.User;

public class HomePageDto
{
    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = null!;
    public List<HomePageFilmDto> Films { get; set; } = null!;
}

public class HomePageFilmDto
{
    public int FilmId { get; set; }
    public string FilmName { get; set; } = null!;
    public byte[] Poster { get; set; } = null!;
}