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

        //Inventory methods
        public bool PickUp(Item item)
        {
            if (Inventory == null)
                return false;

            int newWeight = GetCurrentWeight() + (int)item.Weight;
            if (newWeight > Inventory.MaxWeight)
            {
                Console.WriteLine($"Cannot carry {item.Name} - too heavy! ({newWeight}/{Inventory.MaxWeight})");
                return false;
            }

            Inventory.AddItem(item);
            Console.WriteLine($"{Name} picked up {item.Name}.");
            return true;
        }

        public void Drop(Item item)
        {
            if (Inventory == null) return;

            if (Inventory.RemoveItem(item))
            {
                Console.WriteLine($"{Name} dropped {item.Name}.");
            }
        }

        public void Equip(Item item)
        {
            if (Inventory == null || Equipment == null) return;

            if (!Inventory.Items.Contains(item))
            {
                Console.WriteLine($"{item.Name} isn't in the backpack.");
                return;
            }

            if (item is not Weapon && item is not Armor)
            {
                Console.WriteLine($"{item.Name} can't be equipped.");
                return;
            }

            Inventory.RemoveItem(item);
            Equipment.AddItem(item);
            Console.WriteLine($"{Name} equipped {item.Name}.");
        }

        public void Unequip(Item item)
        {
            if (Inventory == null || Equipment == null) return;

            if (!Equipment.Items.Contains(item))
            {
                Console.WriteLine($"{item.Name} isn't equipped.");
                return;
            }

            Equipment.RemoveItem(item);
            Inventory.AddItem(item);
            Console.WriteLine($"{Name} unequipped {item.Name}.");
        }

        public void UseItem(Item item)
        {
            if (item is not Consumable consumable)
            {
                Console.WriteLine($"{item.Name} can't be used.");
                return;
            }

            switch (consumable.Effect)
            {
                case ConsumableEffect.Heal:
                    TemporaryHealth += consumable.EffectStrength;
                    Console.WriteLine($"{Name} uses {item.Name} and heals for {consumable.EffectStrength} health!");
                    break;
                case ConsumableEffect.StrengthBoost:
                    TemporaryStrength += consumable.EffectStrength;
                    Console.WriteLine($"{Name} uses {item.Name} and gains {consumable.EffectStrength} temporary strength!");
                    break;
                case ConsumableEffect.DefenseBoost:
                    TemporaryDefense += consumable.EffectStrength;
                    Console.WriteLine($"{Name} uses {item.Name} and gains {consumable.EffectStrength} temporary defense!");
                    break;
                case ConsumableEffect.AbilityRestore:
                    var abilities = Abilities.OfType<PlayerAbility>().ToList();
                    if (!abilities.Any())
                    {
                        Console.WriteLine($"{Name} has no abilities to restore.");
                        break;
                    }

                    Console.WriteLine("Choose an ability to restore:");
                    for (int i = 0; i < abilities.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {abilities[i].Name} ({abilities[i].Uses} uses)");
                    }

                    Console.Write("Enter number: ");
                    if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > abilities.Count)
                    {
                        Console.WriteLine("Invalid choice. Restoring the first ability.");
                        choice = 1;
                    }

                    var ability = abilities[choice - 1];
                    ability.Uses += consumable.EffectStrength;
                    Console.WriteLine($"{Name} uses {item.Name} and restores {ability.Name}'s uses by {consumable.EffectStrength}.");
                    break;
                default:
                    Console.WriteLine($"{item.Name} has an unknown effect.");
                    break;
            }


        }

        public int GetCurrentWeight()
        {
            return Inventory?.Items.Sum(i => (int)i.Weight) ?? 0;
        }
    }
    
}
