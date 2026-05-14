using Microsoft.EntityFrameworkCore;
using EFCoreRPGEntities.Data;
using EFCoreRPGEntities.Models;
using EFCoreRPGEntities.Models.Abilities;
using EFCoreRPGEntities.Models.Containers;
using EFCoreRPGEntities.Models.Items;

namespace EFCoreRPG.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly CombatEngine _combat;
    private readonly MenuEngine _menu;

    private readonly Random random = new Random();


    public GameEngine(GameContext context, CombatEngine combat, MenuEngine menu)
    {
        _context = context;
        _combat = combat;
        _menu = menu;
    }

    public void Start()
    {

        while (true)
        {
            var choice = _menu.MainMenu();

            switch (choice)
            {
                case "1":
                    BeginAdventure();
                    break;
                case "2":
                    DisplayRooms();
                    break;
                case "3":
                    DisplayCharacters();
                    break;
                case "4":
                    AddRoom();
                    break;
                case "5":
                    AddCharacter();
                    break;
                case "6":
                    FindCharacter();
                    break;
                case "7":
                    LevelUpCharacter();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }        
    }
    public void BeginAdventure()
    {
        //Main game logic goes here, using other methods to handle specific actions like moving, combat, etc.

        bool playing = true;

        while (playing)
        {
            var player = ChooseAdventurer();


            while (true)
            {
                //TODO: Figure out why abilities aren't showing up in the adventure menu, though they are in the data.
                DisplayCurrentRoom(player);

                var choice = _menu.AdventureMenu();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter direction to move (N/S/E/W): ");
                        var direction = Console.ReadLine();
                        MovePlayer(player, direction);
                        break;
                    case "2":
                        ManageInventory(player);
                        break;
                    case "3":
                        Console.WriteLine($"Character: {player.Name}, Level: {player.Level}," +
                            $" Health: {player.TemporaryHealth}/{player.Health}, Strength: {player.Strength}, Defense: {player.Defense}");
                        break;
                    case "4":
                        if (!player.Abilities.Any())
                        {
                            Console.WriteLine("You have no abilities.");
                            break;
                        }
                        Console.WriteLine($"Abilities:");
                        foreach (var ability in player.Abilities)
                        {
                            Console.WriteLine($"- {ability.Name}: {((PlayerAbility)ability).Uses} uses left");
                        }
                        break;
                    case "5":
                        if (player.Room.Monsters.Any(m => m.IsAlive))
                        {
                            Console.WriteLine("You can't rest while there are living monsters in the room!");
                            break;
                        }
                        Console.WriteLine("Resting to recover health...");
                        player.TemporaryHealth = player.Health; 
                        _context.SaveChanges();
                        Console.WriteLine("Health fully recovered!");
                        break;
                    case "6":
                        //Create a list of the living monsters in the room
                        var aliveMonsters = player.Room.Monsters.Where(m => m.IsAlive).ToList();
                        //if no monsters to fight, break out of combat menu and return to adventure menu
                        if (!aliveMonsters.Any())
                        {
                            Console.WriteLine("There are no monsters to fight here.");
                            break;
                        }

                        Monster selectedMonster;
                        //if only one monster, select it immediately
                        if (aliveMonsters.Count == 1)
                        {
                            selectedMonster = aliveMonsters[0];
                        }
                        //else prompt to choose a specific monster to fight
                        else
                        {
                            Console.WriteLine("Choose a monster to fight:");
                            for (int i = 0; i < aliveMonsters.Count; i++)
                            {
                                Console.WriteLine($"\t{i + 1}: {aliveMonsters[i].Name}");
                            }

                            if (!int.TryParse(Console.ReadLine(), out var monsterChoice) ||
                                monsterChoice < 1 || monsterChoice > aliveMonsters.Count)
                            {
                                Console.WriteLine("Invalid choice.");
                                break;
                            }

                            selectedMonster = aliveMonsters[monsterChoice - 1];
                        }

                        //begin combat with the selected monster
                        Console.WriteLine("Beginning combat...");
                        var playerDefeated = _combat.StartCombat(player, selectedMonster);
                        //if defeated return to main menu, otherwise return to adventure menu
                        if (playerDefeated)
                        {
                            Console.WriteLine("Returning to main menu...");
                            playing = false;
                            return;
                        }
                        break;
                    case "0":
                        //exit adventure and return to main menu
                        Console.WriteLine("Exiting adventure...");
                        playing = false;
                        return;
                    default:
                        //default option, repeating adventure menu
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }


            }
        }


    }
    public void AddRoom()
    {
        Console.Write("Enter room name: ");
        var name = Console.ReadLine();
        Console.Write("Enter room description: ");
        var description = Console.ReadLine();
        var room = new Room { Name = name ?? "Default Room", Description = description ?? "Default Description" };
        _context.Rooms.Add(room);
        _context.SaveChanges();
        Console.WriteLine($"Room '{name}' added successfully.");
    }

    public void AddCharacter()
    {
        Console.Write("Enter character name: ");
        var name = Console.ReadLine();

        Console.Write("Enter character level: ");
        var level = int.Parse(Console.ReadLine() ?? "1");
        
        Console.Write("Enter room ID for the character: ");
        var roomId = int.Parse(Console.ReadLine());

        var room = _context.Rooms.Find(roomId);
        if (room == null)
        {
            Console.WriteLine("Room not found. Character not added.");
            return;
        }

        var character = new Player { Name = name, Level = level, RoomId = roomId };

        _context.Characters.Add(character);
        _context.SaveChanges();

        Console.WriteLine($"Character '{name}' added successfully to room '{room.Name}'.");

    }
    public void DisplayRooms()
    {
        var rooms = _context.Rooms.Include(r => r.Players).ToList();

        foreach (var room in rooms)
        {
            Console.WriteLine($"Room: {room.Name} - {room.Description}");
            foreach (var player in room.Players)
            {
                Console.WriteLine($"    Character: {player.Name}, Level: {player.Level}");
            }
        }
    }

    public void DisplayCharacters()
    {
        var characters = _context.Characters.ToList();
        if (characters.Any())
        {
            Console.WriteLine("\nCharacters:");
            foreach (var character in characters.Where(c => c is Player))
            {
                Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
            }
        }
        else
        {
            Console.WriteLine("No characters available.");
        }
    }

    public void FindCharacter()
    {
        Console.Write("Enter character name to find: ");
        var name = Console.ReadLine();

        var character = _context.Characters
            .FirstOrDefault(c => c.Name.Contains(name));

        if (character != null)
        {
            Console.WriteLine($"Character found: ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
        }
        else
        {
            Console.WriteLine("Character not found.");
        }
    }

    public void LevelUpCharacter()
    {
        Console.Write("Enter the name of the character to level up: ");
        var name = Console.ReadLine();

        var character = _context.Characters.FirstOrDefault(c => c.Name == name);

        if (character != null)
        {
            character.Level++;
            _context.SaveChanges();
            Console.WriteLine($"Character '{name}' leveled up to level {character.Level}.");
        }
        else
        {
            Console.WriteLine("Character not found.");
        }
    }

    public Player ChooseAdventurer()
    {
        Console.WriteLine("Choose which character to start the adventure with: ");
        var characters = _context.Characters
            .OfType<Player>()
            .Include(p => p.Inventory)
                .ThenInclude(i => i.Items)
            .Include(p => p.Equipment)
                .ThenInclude(e => e.Items)
            .Include(p => p.Equipment)
                .ThenInclude(e => e.Weapon)
            .Include(p => p.Equipment)
                .ThenInclude(e => e.Armor)
            .ToList();

        if (!characters.Any())
        {
            Console.WriteLine("No characters available.");
            return null;
        }

        for (int i = 0; i < characters.Count; i++)
        {
            Console.WriteLine($"\t{i + 1}: {characters[i].Name} (Level {characters[i].Level})");
        }

        var playerChoice = int.Parse(Console.ReadLine() ?? "0");
        var selectedPlayer = characters.ElementAtOrDefault(playerChoice - 1);

        if (selectedPlayer != null)
        {
            Console.WriteLine($"Starting adventure with {selectedPlayer.Name} (Level {selectedPlayer.Level})");
        }
        else
        {
            Console.WriteLine("Character not found.");
            return null;
        }

        Console.WriteLine($"Welcome, {selectedPlayer.Name}! Your adventure begins in the {selectedPlayer.Room?.Name}.");
        return selectedPlayer;
    }

    public void MovePlayer(Player player, string direction)
    {
        var currentRoom = _context.Rooms
            .Include(r => r.NorthRoom)
            .Include(r => r.SouthRoom)
            .Include(r => r.EastRoom)
            .Include(r => r.WestRoom)
            .FirstOrDefault(r => r.Id == player.RoomId);

        if (currentRoom == null)
        {
            Console.WriteLine("Current room not found.");
            return;
        }

        Room nextRoom = direction.ToUpper() switch
        {
            "N" => currentRoom.NorthRoom,
            "S" => currentRoom.SouthRoom,
            "E" => currentRoom.EastRoom,
            "W" => currentRoom.WestRoom,
            _ => null
        };

        if (nextRoom == null)
        {
            Console.WriteLine("You can't move in that direction.");
            return;
        }

        player.RoomId = nextRoom.Id;
        _context.SaveChanges();

        Console.WriteLine($"You move {direction} to {nextRoom.Name}.");
        Console.WriteLine(nextRoom.Description);
    }

    public void DisplayCurrentRoom(Player player)
    {
        var room = _context.Rooms
            .Include(r => r.Players)
            .Include(r => r.Monsters)
            .FirstOrDefault(r => r.Id == player.RoomId);

        if (room == null)
        {
            Console.WriteLine("Current room not found.");
            return;
        }

        Console.WriteLine($"\n==== {room.Name} ===="); 
        Console.WriteLine(room.Description);

        var exits = new List<string>();
        if (room.NorthRoomId.HasValue) exits.Add("North");
        if (room.SouthRoomId.HasValue) exits.Add("South");
        if (room.EastRoomId.HasValue) exits.Add("East");
        if (room.WestRoomId.HasValue) exits.Add("West");
        Console.WriteLine($"Exits: {string.Join(", ", exits)}");

        //TODO: Update to show that the monster in the room is dead or alive, and update the combat logic to account for multiple monsters in a room.
        if (room.Monsters.Any())
        {
            var monsterStatuses = room.Monsters.Select(m =>
            {
                var status = m.IsAlive ? "Alive" : "Dead";
                return $"{m.Name} ({status})";
            });

            Console.WriteLine($"Monsters here: {string.Join(", ", monsterStatuses)}");
        }
    }

    private void ManageInventory(Player player)
    {
        EnsureInventoryLoaded(player);

        while (true)
        {
            var choice = _menu.InventoryMenu();

            switch (choice)
            {
                case "1":
                    DisplayAllItems(player);
                    break;
                case "2":
                    UseInventoryItem(player);
                    break;
                case "3":
                    EquipInventoryItem(player);
                    break;
                case "4":
                    UnequipItem(player);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }

    private void EnsureInventoryLoaded(Player player)
    {
        //TODO: Review this to understand loading of context references
        _context.Entry(player).Reference(p => p.Inventory).Load();
        _context.Entry(player).Reference(p => p.Equipment).Load();

        var created = false;

        if (player.Inventory == null)
        {
            player.Inventory = new Inventory { ContainerType = "Inventory", MaxWeight = 100 };
            _context.Add(player.Inventory);
            created = true;
        }

        if (player.Equipment == null)
        {
            player.Equipment = new Equipment { ContainerType = "Equipment" };
            _context.Add(player.Equipment);
            created = true;
        }

        if (player.Inventory != null)
        {
            _context.Entry(player.Inventory).Collection(i => i.Items).Load();
        }

        if (player.Equipment != null)
        {
            _context.Entry(player.Equipment).Collection(e => e.Items).Load();
            _context.Entry(player.Equipment).Reference(e => e.Weapon).Load();
            _context.Entry(player.Equipment).Reference(e => e.Armor).Load();
        }

        if (created)
        {
            _context.SaveChanges();
        }
    }

    private void DisplayAllItems(Player player)
    {
        Console.WriteLine("\n--- Inventory Items ---");
        if (player.Inventory?.Items.Any() == true)
        {
            foreach (var item in player.Inventory.Items)
            {
                Console.WriteLine($"- {item.Name} ({item.GetType().Name})");
            }
        }
        else
        {
            Console.WriteLine("No items in inventory.");
        }

        Console.WriteLine("\n--- Equipped Items ---");
        if (player.Equipment?.Items.Any() == true)
        {
            foreach (var item in player.Equipment.Items)
            {
                Console.WriteLine($"- {item.Name} ({item.GetType().Name})");
            }
        }
        else
        {
            Console.WriteLine("No items equipped.");
        }
    }

    private void UseInventoryItem(Player player)
    {
        var items = player.Inventory?.Items
            .OfType<Consumable>()
            .Cast<Item>()
            .ToList() ?? new List<Item>();

        if (!items.Any())
        {
            Console.WriteLine("No usable items in inventory.");
            return;
        }

        var selected = PromptForItemSelection(items, "Choose an item to use:");
        if (selected == null)
        {
            return;
        }

        player.UseItem(selected);
        player.Inventory?.RemoveItem(selected);
        _context.SaveChanges();
    }

    private void EquipInventoryItem(Player player)
    {
        var items = player.Inventory?.Items
            .Where(i => i is Weapon || i is Armor)
            .ToList() ?? new List<Item>();

        if (!items.Any())
        {
            Console.WriteLine("No equippable items in inventory.");
            return;
        }

        var selected = PromptForItemSelection(items, "Choose an item to equip:");
        if (selected == null)
        {
            return;
        }

        player.Equip(selected);
        _context.SaveChanges();
    }

    private void UnequipItem(Player player)
    {
        var items = player.Equipment?.Items.ToList() ?? new List<Item>();
        if (!items.Any())
        {
            Console.WriteLine("No items equipped.");
            return;
        }

        var selected = PromptForItemSelection(items, "Choose an item to unequip:");
        if (selected == null)
        {
            return;
        }

        player.Unequip(selected);
        _context.SaveChanges();
    }

    private Item? PromptForItemSelection(IReadOnlyList<Item> items, string prompt)
    {
        Console.WriteLine(prompt);
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i].Name}");
        }

        Console.Write("Enter number: ");
        if (!int.TryParse(Console.ReadLine(), out var choice) ||
            choice < 1 || choice > items.Count)
        {
            Console.WriteLine("Invalid choice.");
            return null;
        }

        return items[choice - 1];
    }
}