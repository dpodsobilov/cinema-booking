namespace Data.Models;

public class PlaceType
{
    public int PlaceTypeId { get; set; }
    public string? PlaceName { get; set; } = null!;
    public decimal DefaultCost { get; set; }
    
    // PlaceTypeID -> FK в Place
    public ICollection<Place> Places { get; set; } = null!;
}