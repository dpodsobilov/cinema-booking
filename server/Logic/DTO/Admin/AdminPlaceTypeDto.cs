namespace Logic.DTO.Admin;

public class AdminPlaceTypeDto
{
    public int TypeId { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public decimal Cost { get; set; }
}