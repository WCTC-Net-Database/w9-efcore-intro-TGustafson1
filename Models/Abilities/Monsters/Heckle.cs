using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W09.Models.Abilities.Monsters
{
    internal class Heckle : MonsterAbility
    {
        public Heckle()
        {
            Name = "Heckle";
            Description = "The monster taunts the target, lowering their morale and making them more vulnerable to attacks.";
            AbilityLevel = 1;
        }

        public override void Activate(Character user, Character target)
        {
            Console.WriteLine($"{user.Name} uses {Name} on {target.Name}!");
            Console.WriteLine($"/t{target.Name} is heckled and loses {AbilityLevel} defense!");
            target.Defense -= AbilityLevel;

        }
    }
}
