namespace Logic.DTO.Admin;

public class AdminSessionDto
{
    public int SessionId { get; set; }
    public int FilmId { get; set; }
    public string FilmName { get; set; } = null!;
    public string CinemaName { get; set; } = null!;
    public decimal DataTimeCoefficient { get; set; }
    public int CinemaHallId { get; set; }
    public string CinemaHallName { get; set; } = null!;
    public DateTime DataTimeSession { get; set; }
    public string SessionDate { get; set; } = null!;
    public string SessionTime { get; set; } = null!;
}