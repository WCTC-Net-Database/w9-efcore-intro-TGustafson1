using W09.Data;
using W09.Services;

namespace W09;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new GameContext())
        {
            // Seed the data if necessary
            context.Seed();

            // Initialize GameEngine and Menu
            var gameEngine = new GameEngine(context);
            var menu = new Menu(gameEngine);

            // Show the menu
            menu.Show();
        }
    }
}