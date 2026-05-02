using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W09.Models.Abilities;

namespace W09.Models
{
    public class Player : Character
    {
        public int Experience { get; set; }

        public new ICollection<PlayerAbility> Abilities { get; set; } = new List<PlayerAbility>();

        public override void Attack(ICharacter target)
        {
            //TODO: Double Check correct damage calculations
            Console.WriteLine($"{Name} strikes {target.Name} with a mighty blow!");
            int playerStrength = Strength + TemporaryStrength;
            int targetDefense = target.Defense + target.TemporaryDefense;
            int damage = 5 + playerStrength - targetDefense;
            target.TemporaryHealth -= damage;
            Console.WriteLine($"{target.Name} takes {damage} damage and has {Math.Max(target.TemporaryHealth, 0)} health left.");
        }
    }
}
