using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models.Items
{
    public class KeyItem : Item
    {
        public string? KeyId { get; set; }

        public override void Use(Player player)
        {
            Console.WriteLine($"{player.Name} uses {Name}, but it has no effect when used directly. It can be used to unlock specific doors or chests.");
        }
    }
}
