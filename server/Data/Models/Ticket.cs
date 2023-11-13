namespace Data.Models;

public class Ticket
{
    public int TicketId { get; set; }
    public decimal Price { get; set; }
    
    //FK - UserID
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    //FK - SessionId
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;
    //FK - PlaceId
    public int PlaceId { get; set; }
    public Place Place { get; set; } = null!;
}