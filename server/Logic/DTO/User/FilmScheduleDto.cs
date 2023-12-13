namespace Logic.DTO.User;

public class FilmScheduleDto
{
    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = null!;
    public int CinemaHallId { get; set; }
    public string CinemaHallName { get; set; } = null!;
    public int SessionId { get; set; }
    public DateTime  DataTimeSession { get; set; }
    
}