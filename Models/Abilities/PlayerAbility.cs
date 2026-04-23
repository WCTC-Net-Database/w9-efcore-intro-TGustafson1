using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W09.Models.Abilities
{
    public class PlayerAbility : Ability
    {
        public int Kick { get; set; }

        public override void Activate(Character user, Character target)
        {
            Console.WriteLine($"{user.Name} uses {Name} on {target.Name}!");
            Console.WriteLine($"/t{Description}");
            Console.WriteLine($"/t{target.Name} is pushed back {Kick} feet!");
        }

    }
}
