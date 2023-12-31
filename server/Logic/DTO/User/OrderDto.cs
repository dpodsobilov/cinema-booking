namespace Logic.DTO.User;

public class OrderDto
{
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public ICollection<PlaceAndCostDto> PlaceAndCost { get; set; } = null!;
}

public class PlaceAndCostDto
{
    public int PlaceId { get; set; }
    public decimal Price { get; set; }
}