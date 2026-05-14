using EFCoreRPGEntities.Models.Containers;

namespace EFCoreRPGEntities.Models
{
    public class Door : ILockable
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Door";

        public int RoomAId { get; set; }
        public int RoomBId { get; set; }

        public virtual Room RoomA { get; set; } = null!;
        public virtual Room RoomB { get; set; } = null!;

        public bool IsLocked { get; set; }
        public int? RequiredKeyItemId { get; set; }
    }
}