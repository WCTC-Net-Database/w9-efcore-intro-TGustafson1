using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models.Items
{
    public class Weapon : Item
    {
        public int AttackPower { get; set; }

        public override void Use(Player player)
        {
            Console.WriteLine($"{player.Name} equips {Name} and gains {AttackPower} attack power.");
        }
    }
}
