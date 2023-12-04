namespace Logic.DTO.Admin;

public class AdminSessionDto
{
    public int SessionId { get; set; }
    public string FilmName { get; set; } = null!;
    public string CinemaName { get; set; } = null!;
    public string CinemaHallName { get; set; } = null!;
    public string SessionDate { get; set; } = null!;
    public string SessionTime { get; set; } = null!;
}