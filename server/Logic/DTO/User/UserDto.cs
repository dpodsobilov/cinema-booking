namespace Logic.DTO.User;

public class UserDto
{
    public int UserId { get; set; }
    public int Role { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
}