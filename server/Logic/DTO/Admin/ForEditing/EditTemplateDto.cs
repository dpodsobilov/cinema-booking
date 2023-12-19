namespace Logic.DTO.Admin.ForEditing;

public class EditTemplateDto
{
    public int CinemaHallTypeId { get; set; }
    public string CinemaHallTypeName { get; set; } = null!;
    public List<List<int>> TemplatePlaceTypes { get; set; } = null!;
}