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
        public void StartCombat(Player player, Goblin goblin)
        {
            bool combatEnded = false;

            while (!combatEnded)
            {
                // Player's turn
                Console.WriteLine("Player's turn:");
                player.Attack(goblin);
                if (goblin.Health <= 0)
                {
                    Console.WriteLine($"{goblin.Name} defeated! You win!");
                    combatEnded = true;
                    continue;
                }
                // Goblin's turn
                Console.WriteLine($"{goblin.Name}'s turn:");
                goblin.Attack(player);
                if (player.Health <= 0)
                {
                    Console.WriteLine($"You have been defeated by {goblin.Name}. Game over.");
                    combatEnded = true;
                }
            }
        }
    }
}
