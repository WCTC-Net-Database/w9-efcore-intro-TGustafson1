namespace W09.Models;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }

    // Foreign key to Room
    public int RoomId { get; set; }

    // Navigation property to Room
    public virtual Room Room { get; set; }
}