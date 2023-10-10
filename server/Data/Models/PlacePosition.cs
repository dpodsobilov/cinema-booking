namespace Data.Models;

public class PlacePosition
{
    public int PlacePositionId { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    
    // PlacePositionID -> FK в Place
    public ICollection<Place> Places { get; set; } = null!;
}