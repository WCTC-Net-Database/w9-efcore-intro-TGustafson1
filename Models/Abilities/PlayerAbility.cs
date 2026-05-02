using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W09.Models.Abilities
{
    public class PlayerAbility : Ability
    {
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
                Console.WriteLine($"{Name} {Description}");
                Uses--;
            }
        }

    }
}
