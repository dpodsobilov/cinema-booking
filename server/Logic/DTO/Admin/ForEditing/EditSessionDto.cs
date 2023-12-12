namespace Logic.DTO.Admin.ForEditing;

public class EditSessionDto
{
    public int SessionId { get; set; }
    public DateTime  DataTimeSession { get; set; }
    public decimal DataTimeCoefficient { get; set; }
    public int FilmId { get; set; }
    public int CinemaHallId { get; set; }
}