using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCoreRPGEntities.Data;
using EFCoreRPGEntities.Models;
using EFCoreRPGEntities.Models.Abilities;
using EFCoreRPGEntities.Models.Items;

namespace EFCoreRPG.Services
{
    public class CombatEngine
    {

        private readonly Random _random = new Random();
        private bool combatEnded = false;


        public bool StartCombat(Player player, Monster monster)
        {
            combatEnded = false;
            InitializeStats(player, monster);

            Console.WriteLine($"You face up against a {monster.Name}!\n");

            while (!combatEnded)
            {
                Console.WriteLine($"Player health: {player.TemporaryHealth}/{player.Health}\t{monster.Name} health: {monster.TemporaryHealth}/{monster.Health}");
                Console.WriteLine();
                // Player's turn
                Console.WriteLine("Player's turn:");
                PlayerTurn(player, monster);

                if (combatEnded) continue;

                if (monster.TemporaryHealth <= 0)
                {
                    Console.WriteLine($"{monster.Name} defeated! You win!");
                    monster.IsAlive = false;
                    combatEnded = true;
                    continue;
                }

                // Monster's turn
                Console.WriteLine($"{monster.Name}'s turn:");
                MonsterTurn(monster, player);
                if (player.TemporaryHealth <= 0)
                {
                    Console.WriteLine($"You have been defeated by {monster.Name}. Game over.");
                    combatEnded = true;
                }

                // After combat ends, reset temporary stat changes and grant experience if player won
                if (combatEnded)
                {
                    //TODO: Review experience gain and leveling process
                    if (monster.TemporaryHealth <= 0)
                        player.Experience += monster.AggressionLevel * 10;
                    player.TemporaryDefense = player.Defense;
                    player.TemporaryStrength = player.Strength;
                }
            }

            return player.TemporaryHealth <= 0;
        }

        public void PlayerTurn(Player player, Monster monster)
        {
            while (true)
            {
                //choice between attack, ability, item, or flee
                Console.WriteLine("1. Attack\n2. Use Ability\n3. Use Item\n4. Flee");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();
                Console.WriteLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine($"{player.Name} attacks!");
                        player.Attack(monster);
                        return;
                    case "2":
                        if (player.Abilities.Any())
                        {
                            Console.WriteLine("Choose an ability:");
                            for (int i = 0; i < player.Abilities.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {player.Abilities.ElementAt(i).Name}: {((PlayerAbility)player.Abilities.ElementAt(i)).Uses} uses left");
                            }
                            var abilityChoice = Console.ReadLine();
                            if (int.TryParse(abilityChoice, out int abilityIndex) && abilityIndex > 0 && abilityIndex <= player.Abilities.Count)
                            {
                                var ability = player.Abilities.ElementAt(abilityIndex - 1);
                                player.UseAbility(ability, monster);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice. Defaulting to attack.");
                                player.Attack(monster);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No abilities available. Defaulting to attack.");
                            player.Attack(monster);
                        }
                        return;
                    case "3":
                        if (UseConsumableItem(player))
                        {
                            return;
                        }
                        break;
                    case "4":
                        //regenerate monster's stats to full if player flees
                        monster.TemporaryHealth = monster.Health;
                        monster.TemporaryDefense = monster.Defense;
                        monster.TemporaryStrength = monster.Strength;

                        Console.WriteLine("You flee the battle cowardly! The monster regains its strength.");
                        combatEnded = true;
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private bool UseConsumableItem(Player player)
        {
            var consumables = player.Inventory?.Items
                .OfType<Consumable>()
                .Cast<Item>()
                .ToList() ?? new List<Item>();

            if (!consumables.Any())
            {
                Console.WriteLine("No usable items in inventory.");
                return false;
            }

            Console.WriteLine("Choose an item to use:");
            for (int i = 0; i < consumables.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {consumables[i].Name}");
            }
            Console.WriteLine("0. Back");

            Console.Write("Enter number: ");
            if (!int.TryParse(Console.ReadLine(), out var choice))
            {
                Console.WriteLine("Invalid choice.");
                return false;
            }

            if (choice == 0)
            {
                return false;
            }

            if (choice < 1 || choice > consumables.Count)
            {
                Console.WriteLine("Invalid choice.");
                return false;
            }

            var selected = consumables[choice - 1];
            player.UseItem(selected);
            player.Inventory?.RemoveItem(selected);
            return true;
        }

        public void MonsterTurn(Monster attacker, Player defender)
        {
            // 30% chance to use an ability if available, otherwise attack
            int abilityChance = 30;

            int roll = _random.Next(1, 101);

            if (roll <= abilityChance && attacker.Abilities.Any())
            {
                int abilityIndex = _random.Next(attacker.Abilities.Count);
                var ability = attacker.Abilities.ElementAt(abilityIndex);
                attacker.UseAbility(ability, defender);
            }
            else
            {
                Console.WriteLine($"{attacker.Name} attacks!");
                attacker.Attack(defender);
            }

            Console.WriteLine();
        }

        public void InitializeStats(Character player, Monster monster)
        {
            player.TemporaryHealth = player.Health;
            player.TemporaryDefense = player.Defense;
            player.TemporaryStrength = player.Strength;

            monster.TemporaryHealth = monster.Health;
            monster.TemporaryDefense = monster.Defense;
            monster.TemporaryStrength = monster.Strength;
        }
    }
}
