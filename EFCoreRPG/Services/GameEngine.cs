using Microsoft.EntityFrameworkCore;
using EFCoreRPGEntities.Data;
using EFCoreRPGEntities.Models;
using EFCoreRPGEntities.Models.Abilities;

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

            DisplayCurrentRoom(player);

            while (true)
            {
                var choice = _menu.AdventureMenu();
                switch (choice)
                {
                    case "1":
                        //TODO: Set up map and room connections, then implement movement logic here
                        Console.Write("Enter direction to move (N/S/E/W): ");
                        var direction = Console.ReadLine();
                        MovePlayer(player, direction);
                        break;
                    case "2":
                        //TODO: Future inventory system to manage items and equipment. 
                        Console.WriteLine("Inventory feature not implemented yet.");
                        break;
                    case "3":
                        Console.WriteLine($"Character: {player.Name}, Level: {player.Level}, Health: {player.TemporaryHealth}");
                        break;
                    case "4":
                        Console.WriteLine("Resting to recover health...");
                        player.TemporaryHealth = player.Health; //TODO: Revisit healing per round idea
                        _context.SaveChanges();
                        Console.WriteLine("Health fully recovered!");
                        break;
                    case "5":
                        Console.WriteLine("Exiting adventure...");
                        playing = false;
                        return;
                    default:
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
        //TODO: Fix so that it just iterates over players (1,2,3...) rather than using player.ID
        Console.WriteLine("Choose which character to start the adventure with: ");
        foreach (var player in _context.Characters.Where(c => c is Player))
        {
            Console.WriteLine($"\t{player.Id}: {player.Name} (Level {player.Level})");
        }
        var playerId = int.Parse(Console.ReadLine() ?? "0");


        var selectedPlayer = _context.Characters.Find(playerId) as Player;


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

        if (room.Monsters.Any())
        {
            Console.WriteLine($"Monsters here: {string.Join(", ", room.Monsters.Select(m => m.Name))}");
        }
    }
}