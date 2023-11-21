namespace Logic.DTO;

public class OrderDTO
{
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public ICollection<PlaceAndCostDTO> PlaceAndCost { get; set; } = null!;
}

public class PlaceAndCostDTO
{
    public int PlaceId { get; set; }
    public decimal Price { get; set; }
}