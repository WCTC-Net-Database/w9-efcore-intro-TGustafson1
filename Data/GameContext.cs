using Microsoft.EntityFrameworkCore;
using W09.Models;
using W09.Models.Abilities;
using W09.Models.Abilities.Monsters;
using W09.Models.Abilities.Players;

namespace W09.Data;

public class GameContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Ability> Abilities { get; set; }

    public GameContext(DbContextOptions<GameContext> options) : base(options)
    {
    }

    // OLD method of setting up DBContext with OnConfiguring, now we use DI in Startup.cs
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=StartingEFCore;Trusted_Connection=True;");
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TPH for Characters
        modelBuilder.Entity<Character>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<Player>("Player")
            .HasValue<Monster>("Goblin");

        //TPH for Abilities
        modelBuilder.Entity<Ability>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<MonsterAbility>("GoblinAbility")
            .HasValue<PlayerAbility>("PlayerAbility");

        // many-to-many between Characters and Abilities
        modelBuilder.Entity<Character>()
            .HasMany(c => c.Abilities)
            .WithMany(a => a.Characters)
            .UsingEntity(j => j.ToTable("CharacterAbilities"));

        base.OnModelCreating(modelBuilder);
    }
    // Seed Method
    public void Seed()
    {
        if (!Rooms.Any())
        {
            var room1 = new Room { Name = "Entrance Hall", Description = "The main entry." };
            var room2 = new Room { Name = "Treasure Room", Description = "A room filled with treasures." };

            var character1 = new Player
            {
                Name = "Knight",
                Level = 1,
                Room = room1,
                Health = 16,
                Strength = 8,
                Defense = 6,
                Experience = 0
            };
            var character2 = new Player
            {
                Name = "Wizard",
                Level = 2,
                Room = room2,
                Health = 12,
                Strength = 3,
                Defense = 2,
                Experience = 100
            };

            var character3 = new Monster
            {
                Name = "Goblin Grunt",
                Level = 1,
                Room = room1,
                Health = 8,
                Strength = 4,
                Defense = 1,
                AggressionLevel = 5
            };
            var character4 = new Monster
            {
                Name = "Goblin Shaman",
                Level = 2,
                Room = room2,
                Health = 10,
                Strength = 2,
                Defense = 3,
                AggressionLevel = 8
            };

            Rooms.AddRange(room1, room2);
            Characters.AddRange(character1, character2, character3, character4);

            SaveChanges();
        }
    }
}