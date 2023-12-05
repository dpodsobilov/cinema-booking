namespace Data.Models;

public class CinemaHallType
{
    public int CinemaHallTypeId { get; set; }
    public string CinemaHallTypeName { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;
    
    // CinemaHallTypeId -> FK в Place
    public ICollection<Place> Places { get; set; } = null!;
    // CinemaHallTypeId -> FK в CinemaHall
    public ICollection<CinemaHall> CinemaHalls { get; set; } = null!;
    
}