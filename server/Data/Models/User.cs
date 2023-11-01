namespace Data.Models;

public class User {
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public int Role { get; set; }
    
    
    // UserID -> FK в Ticket
    public ICollection<Ticket> Tickets { get; set; } = null!;
}