namespace Logic.DTO.User;

public class AllUsersTicketsDto
{
    public IList<UserTicketsDto> upcomingTickets { get; set; }
    
    public IList<UserTicketsDto> pastTickets { get; set; }
}

public class UserTicketsDto
{
    public int TicketId { get; set; }
    public DateTime Date { get; set; }
    public string FilmName { get; set; } = null!;
    public string CinemaName { get; set; } = null!;
    public string CinemaHallName { get; set; } = null!;
    public string PlaceName { get; set; } = null!;
    
    public string PlaceTypeName { get; set; } = null!;
}