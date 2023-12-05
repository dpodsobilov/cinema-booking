namespace Data.Models;

public class Cinema
{
    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;
    
    // CinemaId -> FK в CinemaHall
    public ICollection<CinemaHall> CinemaHalls { get; set; } = null!;
    
}