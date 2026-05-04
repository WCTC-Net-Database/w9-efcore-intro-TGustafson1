using Microsoft.Extensions.DependencyInjection;
using EFCoreRPGEntities.Data;
using EFCoreRPG.Services;

namespace EFCoreRPG;

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

        var engine = serviceProvider.GetService<GameEngine>();
        
        engine?.Start();
    }
}