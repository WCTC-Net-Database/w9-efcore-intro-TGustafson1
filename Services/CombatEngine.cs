using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W09.Data;
using W09.Models;

namespace W09.Services
{
    public class CombatEngine
    {

        private readonly Random _random = new Random();
        private bool combatEnded = false;


        public void StartCombat(Player player, Monster monster)
        {
            

            InitializeStats(player, monster);

            Console.WriteLine($"You face up against a {monster.Name}!\n");

            while (!combatEnded)
            {
                Console.WriteLine($"Player health: {player.TemporaryHealth}\t{monster.Name} health: {monster.TemporaryHealth}");
                Console.WriteLine();
                // Player's turn
                Console.WriteLine("Player's turn:");
                PlayerTurn(player, monster);

                if (combatEnded) continue;

                if (monster.TemporaryHealth <= 0)
                {
                    Console.WriteLine($"{monster.Name} defeated! You win!");
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

                if (combatEnded)
                {
                    //TODO: Update to "save" player data as needed, such as experience points or permanent stat changes
                    //TODO: Grant experience points based on aggression level of monster defeated
                    player.TemporaryHealth = player.Health;
                    player.TemporaryDefense = player.Defense;
                    player.TemporaryStrength = player.Strength;
                }
            }
        }

        public void PlayerTurn(Player player, Monster monster)
        {
            Console.WriteLine("1. Attack\n2. Use Ability\n3. Flee");
            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();
            Console.WriteLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine($"{player.Name} attacks!");
                    player.Attack(monster);
                    break;
                case "2":
                    if (player.Abilities.Any())
                    {
                        Console.WriteLine("Choose an ability:");
                        for (int i = 0; i < player.Abilities.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {player.Abilities.ElementAt(i).Name}: {player.Abilities.ElementAt(i).Uses} uses left");
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
                    break;
                case "3":
                    Console.WriteLine("You flee the battle cowardly! Game over.");
                    combatEnded = true;
                    break;
            }
        }

        public void MonsterTurn(Character attacker, Character defender)
        {
            int abilityChance = 50;

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
