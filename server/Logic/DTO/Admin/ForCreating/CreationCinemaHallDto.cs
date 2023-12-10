namespace Logic.DTO.Admin.ForCreating;

public class CreationCinemaHallDto
{
    public string CinemaHallName { get; set; } = null!;
    public int CinemaHallTypeId { get; set; }
    public int CinemaId { get; set; }
}