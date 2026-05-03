using Microsoft.EntityFrameworkCore;
using W09.Models;
using W09.Models.Abilities;
using W09.Models.Items;

namespace W09.Data;

public class GameContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Ability> Abilities { get; set; }

    public DbSet<Equipment> Equipment { get; set; }

    public DbSet<Item> Items { get; set; }


    public GameContext(DbContextOptions<GameContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TPH for Characters
        modelBuilder.Entity<Character>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<Player>("Player")
            .HasValue<Monster>("Monster");

        //TPH for Abilities
        modelBuilder.Entity<Ability>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<MonsterAbility>("MonsterAbility")
            .HasValue<PlayerAbility>("PlayerAbility");

        // many-to-many between Characters and Abilities
        modelBuilder.Entity<Character>()
            .HasMany(c => c.Abilities)
            .WithMany(a => a.Characters)
            .UsingEntity(j => j.ToTable("CharacterAbilities"));

        // One-to-many relationships for Equipment
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Weapon)
            .WithMany()
            .HasForeignKey(e => e.WeaponId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Armor)
            .WithMany()
            .HasForeignKey(e => e.ArmorId)
            .OnDelete(DeleteBehavior.Restrict);

        //Room Navigation Relationships
        modelBuilder.Entity<Room>()
            .HasOne(r => r.NorthRoom)
            .WithOne()
            .HasForeignKey<Room>(r => r.NorthRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.SouthRoom)
            .WithOne()
            .HasForeignKey<Room>(r => r.SouthRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.EastRoom)
            .WithMany()
            .HasForeignKey(r => r.EastRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.WestRoom)
            .WithMany()
            .HasForeignKey(r => r.WestRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // Player and Monster relationships to room
        modelBuilder.Entity<Player>()
            .HasOne(p => p.Room)
            .WithMany(r => r.Players)
            .HasForeignKey(p => p.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Monster>()
            .HasOne(m => m.Room)
            .WithMany(r => r.Monsters)
            .HasForeignKey(m => m.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

        base.OnModelCreating(modelBuilder);
    }

    //TODO: Set up seed to be a menu option, clearing database and re-seeding a fresh world.
    public void Seed()
    {

        SaveChanges();

        if (!Rooms.Any())
        {
            var room1 = new Room { Name = "Entrance Hall", Description = "The main entry." };
            var room2 = new Room { Name = "Treasure Room", Description = "A room filled with treasures." };

            Rooms.AddRange(room1, room2);

            var heckle = new MonsterAbility
            {
                Name = "Heckle",
                Description = "taunts the target, reducing their defensive power.",
                AbilityLevel = 1,
                DefenseModifier = -1,
                Characters = { }
            };

            Abilities.AddRange(heckle);


            var character1 = new Player
            {
                Name = "Knight",
                Level = 1,
                Room = room1,
                Health = 16,
                Strength = 5,
                Defense = 3,
                Experience = 0
            };
            var character2 = new Player
            {
                Name = "Wizard",
                Level = 1,
                Room = room2,
                Health = 12,
                Strength = 3,
                Defense = 2,
                Experience = 0
            };

            var character3 = new Monster
            {
                Name = "Goblin Grunt",
                Level = 1,
                Room = room1,
                Health = 8,
                Strength = 4,
                Defense = 1,
                AggressionLevel = 5,
                Abilities = { }
            };
            var character4 = new Monster
            {
                Name = "Goblin Shaman",
                Level = 2,
                Room = room2,
                Health = 10,
                Strength = 2,
                Defense = 3,
                AggressionLevel = 8,
                Abilities = { }
            };

            Characters.AddRange(character1, character2, character3, character4);

            SaveChanges();
        }
    }
}