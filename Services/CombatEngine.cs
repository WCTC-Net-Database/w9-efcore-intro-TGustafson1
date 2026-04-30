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

            while (!combatEnded)
            {

                // Player's turn
                Console.WriteLine("Player's turn:");
                TakeTurn(player, monster);
                if (monster.Health <= 0)
                {
                    Console.WriteLine($"{monster.Name} defeated! You win!");
                    combatEnded = true;
                    continue;
                }

                // Monster's turn
                Console.WriteLine($"{monster.Name}'s turn:");
                TakeTurn(monster, player);
                if (player.Health <= 0)
                {
                    Console.WriteLine($"You have been defeated by {monster.Name}. Game over.");
                    combatEnded = true;
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
                Console.WriteLine($"{attacker.Name} uses {ability.Name}!");
                attacker.UseAbility(ability, defender);
            }
            else
            {
                Console.WriteLine($"{attacker.Name} attacks!");
                attacker.Attack(defender);
            }

        }
    }
}
