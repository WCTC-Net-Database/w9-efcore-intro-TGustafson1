using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCoreRPGEntities.Models.Abilities;

namespace EFCoreRPGEntities.Models
{
    public class Monster : Character
    {
        public int AggressionLevel { get; set; }

        public bool IsAlive { get; set; } = true;

        public void Attack(ICharacter target)
        {
            Console.WriteLine($"{Name} lunges at {target.Name} with ferocity!");
            int targetDefense = (target as Player)?.GetTotalDefense() ?? 0;
            int damage = 2 + TemporaryStrength - targetDefense;
            target.TemporaryHealth -= Math.Max(damage, 0);
            Console.WriteLine($"{target.Name} takes {Math.Max(damage, 0)} damage and has {Math.Max(target.TemporaryHealth, 0)} health left.");
        }
    }
}
