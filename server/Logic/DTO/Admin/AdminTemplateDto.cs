namespace Logic.DTO.Admin;

public class AdminTemplateDto
{
    public List<List<AdminPlaceDto>> AdminPlaceDtos { get; set; }
    
    public AdminTemplateDto(int rows, int numbers)
    {
        AdminPlaceDtos = new List<List<AdminPlaceDto>>();
        for (int row = 0; row < rows; row++)
        {
            AdminPlaceDtos.Add(new List<AdminPlaceDto>());
            for (int number = 0; number < numbers; number++)
            {
                AdminPlaceDtos[row].Add(new AdminPlaceDto());
            }
        }
    }
}

public class AdminPlaceDto
{
    public int PlaceTypeId { get; set; }
    public string Color { get; set; } = null!;
}