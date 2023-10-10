namespace Data.Models;

public class CinemaHall
{
    public int CinemaHallId { get; set; }
    public string? CinemaHallName { get; set; } = null!;
    
    // CinemaHallId -> FK в Session
    public ICollection<Session> Sessions { get; set; } = null!;

    //FK - CinemaHallTypeID
    public int CinemaHallTypeId { get; set; }
    public CinemaHallType CinemaHallType { get; set; } = null!;
    //FK - CinemaId
    public int CinemaId { get; set; }
    public Cinema Cinema { get; set; } = null!;
}