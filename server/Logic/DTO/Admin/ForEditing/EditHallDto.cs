namespace Logic.DTO.Admin.ForEditing;

public class EditHallDto
{
    public int CinemaHallId { get; set; }
    public string CinemaHallName { get; set; } = null!;
    public int CinemaHallTypeId { get; set; }
    public int CinemaId { get; set; }
}