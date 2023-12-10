namespace Logic.DTO.Admin.ForCreating;

public class CreationTemplateDto
{
    public string CinemaHallTypeName { get; set; } = null!;
    public List<List<PlaceForCreationTemplateDto>> TemplatePlaces { get; set; } = null!;
}

public class PlaceForCreationTemplateDto
{
    public int PlaceTypeId { get; set; }
}