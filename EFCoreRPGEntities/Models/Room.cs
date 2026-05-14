using EFCoreRPGEntities.Models.Containers;

namespace EFCoreRPGEntities.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    // FK's - nullable because a room can be empty
    public int? NorthRoomId { get; set; }
    public int? SouthRoomId { get; set; }
    public int? EastRoomId { get; set; }
    public int? WestRoomId { get; set; }

    // Navigation for exits
    public virtual Room NorthRoom { get; set; }
    public virtual Room SouthRoom { get; set; }
    public virtual Room EastRoom { get; set; }
    public virtual Room WestRoom { get; set; }


    // Navigation property to Characters
    public virtual ICollection<Player> Players { get; set; }
    public virtual ICollection<Monster> Monsters { get; set; }
    public virtual ICollection<Chest> Chests { get; set; } = new List<Chest>();

}