using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models.Abilities
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
                //TODO: Account for positive modifiers that increase player's stats instead of decreasing the target's stats
                Console.WriteLine($"{Name} {Description}");
                if (Damage > 0)
                    Console.WriteLine($"{target.Name} takes {Damage} damage!");
                target.TemporaryHealth -= Damage;
                if (DefenseModifier > 0)
                    Console.WriteLine($"{target.Name} loses {DefenseModifier} defense!");
                target.TemporaryDefense += DefenseModifier;
                if (StrengthModifier > 0)
                    Console.WriteLine($"{target.Name} loses {StrengthModifier} strength!");
                target.TemporaryStrength += StrengthModifier;
                Uses--;
            }
        }

    }
}
