using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCoreRPGEntities.Models.Abilities;
using EFCoreRPGEntities.Models.Containers;
using EFCoreRPGEntities.Models.Items;

namespace EFCoreRPGEntities.Models
{
    public class Player : Character
    {
        public int? EquipmentId { get; set; }
        public virtual Equipment? Equipment { get; set; }

        public int? InventoryId { get; set; }

        public virtual Inventory? Inventory { get; set; }
        public int Experience { get; set; }

        public int GetTotalAttack()
        {
            int baseAttack = Level * 2;
            int weaponBonus = Equipment?.Weapon is Weapon weapon ? weapon.AttackPower : 0;
            return baseAttack + weaponBonus + TemporaryStrength;
        }

        public int GetTotalDefense()
        {
            return (Equipment?.Armor is Armor armor ? armor.Defense : 0) + TemporaryDefense;
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
