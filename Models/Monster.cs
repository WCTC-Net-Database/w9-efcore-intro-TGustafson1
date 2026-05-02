using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W09.Models.Abilities;

namespace W09.Models
{
    public class Monster : Character
    {
        public int AggressionLevel { get; set; }

        public new ICollection<MonsterAbility> Abilities { get; set; } = new List<MonsterAbility>();

        public override void Attack(ICharacter target)
        {
            //TODO: Double check correct damage calculations
            Console.WriteLine($"{Name} lunges at {target.Name} with ferocity!");
            int monsterStrength = Strength + TemporaryStrength;
            int targetDefense = target.Defense + target.TemporaryDefense;
            int damage = 3 + monsterStrength - targetDefense;
            target.TemporaryHealth -= damage;
            Console.WriteLine($"{target.Name} takes {damage} damage and has {Math.Max(target.TemporaryHealth, 0)} health left.");
        }
    }
}
