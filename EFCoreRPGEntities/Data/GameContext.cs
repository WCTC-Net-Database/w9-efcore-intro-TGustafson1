using Microsoft.EntityFrameworkCore;
using EFCoreRPGEntities.Models;
using EFCoreRPGEntities.Models.Abilities;
using EFCoreRPGEntities.Models.Items;
using EFCoreRPGEntities.Models.Containers;

namespace EFCoreRPGEntities.Data;

public class GameContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Ability> Abilities { get; set; }

    public DbSet<Equipment> Equipment { get; set; }

    public DbSet<Item> Items { get; set; }

    public DbSet<Door> Doors { get; set; }


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

        //TPH for Containers
        modelBuilder.Entity<Container>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<Inventory>("Inventory")
            .HasValue<Equipment>("Equipment")
            .HasValue<Chest>("Chest")
            .HasValue<MonsterLoot>("MonsterLoot");

        //TPH for Items
        modelBuilder.Entity<Item>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<Weapon>("Weapon")
            .HasValue<Armor>("Armor")
            .HasValue<Consumable>("Consumable")
            .HasValue<KeyItem>("KeyItem")
            .HasValue<TrophyItem>("TrophyItem");

        // One-to-many relationship between Container and Item
        modelBuilder.Entity<Item>()
            .HasOne(i => i.Container)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.ContainerId)
            .OnDelete(DeleteBehavior.SetNull);

        //Player inventory and equipment relationships
        modelBuilder.Entity<Player>()
            .HasOne(p => p.Inventory)
            .WithMany()
            .HasForeignKey(p => p.InventoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Player>()
            .HasOne(p => p.Equipment)
            .WithMany()
            .HasForeignKey(p => p.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // many-to-many between Characters and Abilities
        modelBuilder.Entity<Character>()
            .HasMany(c => c.Abilities)
            .WithMany(a => a.Characters)
            .UsingEntity(j => j.ToTable("CharacterAbilities"));

        // One-to-many relationships for Equipment
        //TODO: Ensure only 1 weapon and 1 armor piece can be equipped at once
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

        // Monster to monster loot relationship
        modelBuilder.Entity<Monster>()
            .HasOne(m => m.Loot)
            .WithOne()
            .HasForeignKey<Monster>(m => m.LootId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Monster>()
            .Property(m => m.IsAlive)
            .HasDefaultValue(true);

        modelBuilder.Entity<Container>()
            .HasOne(c => c.ParentContainer)
            .WithMany(c => c.ChildContainers)
            .HasForeignKey(c => c.ParentContainerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Room>()
            .HasMany(r => r.Chests)
            .WithOne(c => c.Room)
            .HasForeignKey(c => c.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

        //Door relationships to rooms
        modelBuilder.Entity<Door>()
            .HasOne(d => d.RoomA)
            .WithMany()
            .HasForeignKey(d => d.RoomAId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Door>()
            .HasOne(d => d.RoomB)
            .WithMany()
            .HasForeignKey(d => d.RoomBId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }

    public void Seed()
    {

        if (!Rooms.Any())
        {
            var room1 = new Room { Name = "Entrance Hall", Description = "The main entry, with paths in all directions." };
            var room2 = new Room { Name = "Treasure Room", Description = "A room filled with sparkling treasures." };
            var room3 = new Room { Name = "Armory", Description = "Racks of rusty weapons line the walls." };
            var room4 = new Room { Name = "Dungeon", Description = "Cold, damp cells for prisoners." };
            var room5 = new Room { Name = "Guard Room", Description = "A post where castle guards used to rest." };
            var room6 = new Room { Name = "Library", Description = "Tall shelves of decayed books." };
            var room7 = new Room { Name = "Secret Passage", Description = "A hidden, narrow corridor in the walls." };
            var room8 = new Room { Name = "Alchemy Lab", Description = "Broken glass and strange smells linger here." };
            var room9 = new Room { Name = "Great Hall", Description = "A massive dining area for long-gone royalty." };
            var room10 = new Room { Name = "Throne Room", Description = "An imposing chamber where the boss awaits." };

            Rooms.AddRange(room1, room2, room3, room4, room5, room6, room7, room8, room9, room10);
            SaveChanges();

            // Wire up the map layout
            room1.NorthRoom = room9; room1.SouthRoom = room6; room1.EastRoom = room3; room1.WestRoom = room5;
            room2.SouthRoom = room3; room2.WestRoom = room9;
            room3.NorthRoom = room2; room3.SouthRoom = room7; room3.WestRoom = room1;
            room4.EastRoom = room5;
            room5.EastRoom = room1; room5.WestRoom = room4;
            room6.NorthRoom = room1; room6.EastRoom = room7;
            room7.NorthRoom = room3; room7.WestRoom = room6;
            room8.EastRoom = room9;
            room9.NorthRoom = room10; room9.SouthRoom = room1; room9.EastRoom = room2; room9.WestRoom = room8;
            room10.SouthRoom = room9;

            SaveChanges();


            var heckle = new MonsterAbility
            {
                Name = "Heckle",
                Description = "taunts the target, reducing their defensive power.",
                AbilityLevel = 1,
                DefenseModifier = -1,
                Characters = new List<Character>()
            };

            var fireball = new PlayerAbility
            {
                Name = "Fireball",
                Description = "hurls a fiery ball that explodes on impact.",
                AbilityLevel = 2,
                Uses = 2,
                Damage = 8,
                Characters = new List<Character>()
            };

            var weaken = new PlayerAbility
            {
                Name = "Weaken",
                Description = "saps the target's strength, making them less effective in combat.",
                AbilityLevel = 1,
                Uses = 3,
                StrengthModifier = -2,
                Characters = new List<Character>()
            };

            var battlecry = new PlayerAbility
            {
                Name = "Battlecry",
                Description = "lets out a fierce shout that frightens the target, lowering their strength!",
                AbilityLevel = 1,
                Uses = 2,
                StrengthModifier = -2,
                Characters = new List<Character>()
            };

            Abilities.AddRange(heckle, fireball, weaken, battlecry);

            var knightInventory = new Inventory { ContainerType = "Inventory", MaxWeight = 80 };
            var knightEquipment = new Equipment { ContainerType = "Equipment" };

            var wizardInventory = new Inventory { ContainerType = "Inventory", MaxWeight = 80 };
            var wizardEquipment = new Equipment { ContainerType = "Equipment" };

            var ironSword = new Weapon
            {
                Name = "Iron Sword",
                Weight = 5,
                Value = 25,
                AttackPower = 2
            };

            var steelSword = new Weapon
            {
                Name = "Steel Sword",
                Weight = 6,
                Value = 45,
                AttackPower = 4
            };

            var leatherArmor = new Armor
            {
                Name = "Leather Armor",
                Weight = 8,
                Value = 20,
                Defense = 2
            };

            var abilityRestorePotion = new Consumable
            {
                Name = "Ability Restore Potion",
                Weight = 1,
                Value = 30,
                Effect = ConsumableEffect.AbilityRestore,
                EffectStrength = 2
            };

            var healthPotion = new Consumable
            {
                Name = "Health Potion",
                Weight = 1,
                Value = 25,
                Effect = ConsumableEffect.Heal,
                EffectStrength = 10
            };

            var ironKey = new KeyItem
            {
                Name = "Iron Key",
                Weight = 1,
                Value = 5
            };

            var goldenKey = new KeyItem
            {
                Name = "Golden Key",
                Weight = 1,
                Value = 50
            };

            var bronzeKey = new KeyItem
            {
                Name = "Bronze Key",
                Weight = 1,
                Value = 10
            };

            var strengthPotion = new Consumable
            {
                Name = "Strength Potion",
                Weight = 1,
                Value = 20,
                Effect = ConsumableEffect.StrengthBoost,
                EffectStrength = 2
            };

            var diamondTrophy = new TrophyItem
            {
                Name = "Diamond-Encrusted Trophy",
                Weight = 1,
                Value = 1000
            };

            var ironHelm = new Armor
            {
                Name = "Iron Helm",
                Weight = 4,
                Value = 15,
                Defense = 3
            };

            knightInventory.AddItem(ironSword);
            knightInventory.AddItem(leatherArmor);

            wizardInventory.AddItem(abilityRestorePotion);



            // create containers
            var armoryChest = new Chest { ContainerType = "Armory Chest", Room = room3 };
            armoryChest.AddItem(steelSword);

            var guardRoomChest = new Chest { ContainerType = "Guard Room Chest", Room = room5 };
            guardRoomChest.AddItem(ironKey);

            var dungeonChest = new Chest { ContainerType = "Dungeon Chest", Room = room4 };
            dungeonChest.AddItem(healthPotion);

            var libraryChest = new Chest { ContainerType = "Library Chest", Room = room6 };
            libraryChest.AddItem(goldenKey);

            var alchemyLabChest = new Chest { ContainerType = "Alchemy Lab Chest", Room = room8 };
            alchemyLabChest.AddItem(bronzeKey);

            var goblinShamanLoot = new MonsterLoot { ContainerType = "Goblin Shaman Loot" };
            goblinShamanLoot.AddItem(strengthPotion);

            var skeletonLoot = new MonsterLoot { ContainerType = "Skeleton Loot" };
            skeletonLoot.AddItem(ironHelm);

            var orcKingLoot = new MonsterLoot { ContainerType = "Orc King Loot" };
            orcKingLoot.AddItem(diamondTrophy);

            Add(goblinShamanLoot);
            Add(skeletonLoot);
            Add(knightInventory);
            Add(knightEquipment);
            Add(wizardInventory);
            Add(wizardEquipment);
            Add(armoryChest);
            Add(guardRoomChest);
            Add(dungeonChest);
            Add(libraryChest);
            Add(alchemyLabChest);
            Add(orcKingLoot);

            Items.AddRange(ironSword, leatherArmor, steelSword, abilityRestorePotion, strengthPotion, healthPotion,
                ironKey, goldenKey, bronzeKey, ironHelm, diamondTrophy);
            SaveChanges();

            armoryChest.IsLocked = true;
            armoryChest.RequiredKeyItemId = bronzeKey.Id;

            var doors = new List<Door>
            {
                new Door { Name = "Entrance Hall - Great Hall Door", RoomA = room1, RoomB = room9 },
                new Door { Name = "Entrance Hall - Library Door", RoomA = room1, RoomB = room6 },
                new Door { Name = "Entrance Hall - Armory Door", RoomA = room1, RoomB = room3 },
                new Door { Name = "Entrance Hall - Guard Room Door", RoomA = room1, RoomB = room5 },
                new Door { Name = "Treasure Room - Armory Door", RoomA = room2, RoomB = room3 },
                new Door { Name = "Treasure Room - Great Hall Door", RoomA = room2, RoomB = room9 },
                new Door { Name = "Armory - Secret Passage Door", RoomA = room3, RoomB = room7 },
                new Door { Name = "Guard Room - Dungeon Door", RoomA = room5, RoomB = room4, IsLocked = true, RequiredKeyItemId = ironKey.Id },
                new Door { Name = "Library - Secret Passage Door", RoomA = room6, RoomB = room7 },
                new Door { Name = "Alchemy Lab - Great Hall Door", RoomA = room8, RoomB = room9 },
                new Door { Name = "Great Hall - Throne Room Door", RoomA = room9, RoomB = room10, IsLocked = true, RequiredKeyItemId = goldenKey.Id }
            };

            Doors.AddRange(doors);

            SaveChanges();

            var character1 = new Player
            {
                Name = "Knight",
                Level = 1,
                Room = room1,
                Health = 16,
                Strength = 5,
                Defense = 3,
                Experience = 0,
                Inventory = knightInventory,
                Equipment = knightEquipment,
                Abilities = new List<Ability> { battlecry }
            };

            var character2 = new Player
            {
                Name = "Wizard",
                Level = 1,
                Room = room2,
                Health = 12,
                Strength = 3,
                Defense = 2,
                Experience = 0,
                Inventory = wizardInventory,
                Equipment = wizardEquipment,
                Abilities = new List<Ability> { fireball, weaken }
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
                IsAlive = true,
                Abilities = new List<Ability> { heckle }
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
                IsAlive = true,
                Abilities = new List<Ability> { heckle },
                Loot = goblinShamanLoot
            };

            // Place an enemy in the boss room
            var character5 = new Monster
            {
                Name = "Orc King",
                Level = 5,
                Room = room10,
                Health = 30,
                Strength = 8,
                Defense = 5,
                AggressionLevel = 10,
                IsAlive = true,
                Abilities = new List<Ability> { heckle },
                Loot = orcKingLoot
            };

            var character6 = new Monster
            {
                Name = "Skeleton",
                Level = 2,
                Room = room4,
                Health = 12,
                Strength = 4,
                Defense = 2,
                AggressionLevel = 6,
                IsAlive = true,
                Abilities = new List<Ability> { },
                Loot = skeletonLoot
            };

            Characters.AddRange(character1, character2, character3, character4, character5, character6);
            SaveChanges();
        }
    }
}