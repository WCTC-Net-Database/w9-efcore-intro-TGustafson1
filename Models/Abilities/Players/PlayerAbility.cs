using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W09.Models.Abilities.Players
{
    public class PlayerAbility : Ability
    {
        public int AbilityLevel { get; set; }
        public int Uses { get; set;  }

        public override void Activate(Character user, Character target)
        {
            if (Uses <= 0)
            {
                Console.WriteLine($"{user.Name} tries to use {Name}, but has no uses left!");
                return;
            }
            if (Uses > 0)
            {
                Console.WriteLine($"{user.Name} uses {Name} on {target.Name}!");
                Console.WriteLine($"/t{target.Name} is pushed back {AbilityLevel} feet!");
            }
        }

    }
}
