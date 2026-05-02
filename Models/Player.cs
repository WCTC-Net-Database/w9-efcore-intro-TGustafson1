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
            Console.WriteLine($"{Name} strikes {target.Name} with a mighty blow!");
            int playerStrength = TemporaryStrength;
            int targetDefense = target.TemporaryDefense;
            int damage = 3 + playerStrength - targetDefense;
            target.TemporaryHealth -= Math.Max(damage, 0);
            Console.WriteLine($"{target.Name} takes {Math.Max(damage, 0)} damage and has {Math.Max(target.TemporaryHealth, 0)} health left.");
        }
    }
}
