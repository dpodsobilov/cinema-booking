using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Session
{
    public int SessionId { get; set; }
    [Column(TypeName = "timestamp")]
    public DateTime  DataTimeSession { get; set; }
    public decimal DataTimeCoefficient { get; set; }
    public bool IsDeleted { get; set; } = false;

    // SessionId -> FK в Ticket
    public ICollection<Ticket> Tickets { get; set; } = null!;
    
    //FK - FilmId
    public int FilmId { get; set; }
    public Film Film { get; set; } = null!;
    //FK - CinemaHallId
    public int CinemaHallId { get; set; }
    public CinemaHall CinemaHall { get; set; } = null!;
}