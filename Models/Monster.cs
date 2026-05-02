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

        public void Attack(Player target)
        {
            Console.WriteLine($"{Name} lunges at {target.Name} with ferocity!");
            int targetDefense = target.GetTotalDefense();
            int damage = 2 + TemporaryStrength - targetDefense;
            target.TemporaryHealth -= Math.Max(damage, 0);
            Console.WriteLine($"{target.Name} takes {Math.Max(damage, 0)} damage and has {Math.Max(target.TemporaryHealth, 0)} health left.");
        }
    }
}
