namespace Data.Models;

public class PlaceType
{
    public int PlaceTypeId { get; set; }
    public string PlaceTypeName { get; set; } = null!;
    public string PlaceTypeColor { get; set; } = null!;
    public decimal DefaultCost { get; set; }
    public bool IsDeleted { get; set; } = false;
    
    // PlaceTypeID -> FK в Place
    public ICollection<Place> Places { get; set; } = null!;
}