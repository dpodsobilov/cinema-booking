namespace Data.Models;

public class Place
{
    public int PlaceId { get; set; }
    
    // PlaceID -> FK в Ticket
    public ICollection<Ticket> Tickets { get; set; } = null!;
    
    //FK - PlaceTypeID
    public int PlaceTypeId { get; set; }
    public PlaceType PlaceType { get; set; } = null!;
    //FK - PlacePositionId
    public int PlacePositionId { get; set; }
    public PlacePosition PlacePosition { get; set; } = null!;
    //FK - CinemaHallTypeId
    public int CinemaHallTypeId { get; set; }
    public CinemaHallType CinemaHallType { get; set; } = null!;
}