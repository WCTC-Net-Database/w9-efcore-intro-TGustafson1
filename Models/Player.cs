using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W09.Models
{
    public class Player : Character
    {
        public int Experience { get; set; }

        public override void Attack(ICharacter target)
        {
            Console.WriteLine($"{Name} strikes {target.Name} with a mighty blow!");
            int damage = Strength + 5 - target.Defense;
            target.Health -= damage;
            Console.WriteLine($"{target.Name} takes {damage} damage and has {target.Health} health left.");
        }
    }
}
