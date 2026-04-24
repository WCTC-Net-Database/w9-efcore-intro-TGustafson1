using Microsoft.Extensions.DependencyInjection;
using W09.Data;
using W09.Services;

namespace W09;

class Program
{
    static void Main(string[] args)
    {

        // Initialize GameEngine
        var serviceCollection = new ServiceCollection();
        Startup.ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameContext>();
            context?.Seed();
        }

        // Resolving circular dependency - in my head it makes the most sense to have the engine rely on context, and menu/output rely on engine

        var menu = serviceProvider.GetService<Menu>();

        menu?.Show();
    }
}