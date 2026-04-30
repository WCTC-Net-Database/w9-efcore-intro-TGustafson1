using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W09.Models
{
    public class Monster : Character
    {
        public int AggressionLevel { get; set; }

        public override void Attack(ICharacter target)
        {
            Console.WriteLine($"{Name} lunges at {target.Name} with ferocity!");
            int damage = Strength + 3 - target.Defense;
            target.Health -= damage;
            Console.WriteLine($"{target.Name} takes {damage} damage and has {target.Health} health left.");
        }
    }
}
