namespace EFCoreRPGEntities.Models.Containers
{
    public interface ILockable
    {
        bool IsLocked { get; set; }
        int? RequiredKeyItemId { get; set; }
    }
}