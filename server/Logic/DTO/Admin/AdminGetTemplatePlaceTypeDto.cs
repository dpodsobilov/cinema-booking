namespace Logic.DTO.Admin;

public class AdminGetTemplatePlaceTypeDto
{
    public int PlaceTypeId { get; set; }
    public string PlaceTypeName { get; set; } = null!;
    public string PlaceTypeColor { get; set; } = null!;
}