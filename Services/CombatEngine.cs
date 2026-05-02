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

        public void StartCombat(Player player, Monster monster)
        {
            bool combatEnded = false;

            //TODO: Ensure stats are initializing properly.
            InitializeStats(player, monster);

            while (!combatEnded)
            {
                //TODO: Add more detailed combat log, such as damage dealt, abilities used, and remaining health after each turn
                
                Console.WriteLine();
                // Player's turn
                Console.WriteLine("Player's turn:");
                TakeTurn(player, monster);
                if (monster.TemporaryHealth <= 0)
                {
                    Console.WriteLine($"{monster.Name} defeated! You win!");
                    combatEnded = true;
                    continue;
                }

                // Monster's turn
                Console.WriteLine($"{monster.Name}'s turn:");
                TakeTurn(monster, player);
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

        public void TakeTurn(Character attacker, Character defender)
        {
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

        public void InitializeStats(Character character, Monster monster)
        {
            character.TemporaryHealth = character.Health;
            character.TemporaryDefense = character.Defense;
            character.TemporaryStrength = character.Strength;

            monster.TemporaryHealth = monster.Health;
            monster.TemporaryDefense = monster.Defense;
            monster.TemporaryStrength = monster.Strength;
        }
    }
}
