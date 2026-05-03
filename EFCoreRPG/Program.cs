using Microsoft.Extensions.DependencyInjection;
using EFCoreRPGEntities.Data;
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

        // Resolving circular dependency

        var menu = serviceProvider.GetService<Menu>();
        
        menu?.Show();
    }
}