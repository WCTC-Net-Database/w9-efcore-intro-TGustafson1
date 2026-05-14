using System;

namespace EFCoreRPGEntities.Models.Items
{
    public class KeyItem : Item
    {
        public override void Use(Player player)
        {
            Console.WriteLine($"{player.Name} uses {Name}, but it has no effect when used directly. It can be used to unlock specific doors or chests.");
        }
    }
}
