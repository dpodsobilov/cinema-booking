namespace Logic.DTO;

public class CinemaHallDto
{
    public List<List<PlaceDto>> PlaceDtos { get; set; }

    public CinemaHallDto(int rows, int numbers)
    {
        PlaceDtos = new List<List<PlaceDto>>();
        for (int row = 0; row < rows; row++)
        {
            PlaceDtos.Add(new List<PlaceDto>());
            for (int number = 0; number < numbers; number++)
            {
                PlaceDtos[row].Add(new PlaceDto());
            }
        }
    }
}

public class PlaceDto
{
    public int PlaceId { get; set; }
    public int PlaceTypeId { get; set; }
    public string PlaceName { get; set; } = null!;
    public string PlaceTypeName { get; set; } = null!;
    public string Color { get; set; } = null!;
    public decimal Cost { get; set; }
}