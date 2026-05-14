using System;

namespace EFCoreRPGEntities.Models.Items
{
    public class TrophyItem : Item
    {
        public override void Use(Player player)
        {
            Console.WriteLine($"{player.Name} admires the {Name}. It marks a legendary victory!");
        }
    }
}