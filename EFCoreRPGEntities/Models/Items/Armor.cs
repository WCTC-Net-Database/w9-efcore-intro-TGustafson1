using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models.Items
{
    public class Armor : Item
    {
        public int Defense { get; set; }

        public override void Use(Player player)
        {
            Console.WriteLine($"{player.Name} equips {Name} and gains {Defense} defense.");
        }
    }
}
