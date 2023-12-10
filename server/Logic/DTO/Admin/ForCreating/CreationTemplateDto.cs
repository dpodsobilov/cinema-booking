namespace Logic.DTO.Admin.ForCreating;

public class CreationTemplateDto
{
    public string CinemaHallTypeName { get; set; } = null!;
    public List<List<int>> TemplatePlaceTypes { get; set; } = null!;
}