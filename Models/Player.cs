using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W09.Models.Abilities;
using W09.Models.Items;

namespace W09.Models
{
    public class Player : Character
    {
        public int? EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }
        public int Experience { get; set; }

        public new ICollection<PlayerAbility> Abilities { get; set; } = new List<PlayerAbility>();

        public int GetTotalAttack()
        {
            int baseAttack = Level * 2; 
            int weaponBonus = Equipment?.Weapon?.Attack ?? 0;
            return baseAttack + weaponBonus + TemporaryStrength;
        }

        public int GetTotalDefense()
        {
            return (Equipment?.Armor?.Defense ?? 0) + TemporaryDefense;
        }

        public override void Attack(ICharacter target)
        {
            Console.WriteLine($"{Name} strikes {target.Name} with a mighty blow!");
            int targetDefense = target.TemporaryDefense;
            int damage = GetTotalAttack() - targetDefense;
            target.TemporaryHealth -= Math.Max(damage, 0);
            Console.WriteLine($"{target.Name} takes {Math.Max(damage, 0)} damage and has {Math.Max(target.TemporaryHealth, 0)} health left.");
        }
    }
}
