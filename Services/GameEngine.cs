using Microsoft.EntityFrameworkCore;
using W09.Data;
using W09.Models;
using W09.Models.Abilities;

namespace W09.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly CombatEngine _combat;

    private readonly Random random = new Random();


    public GameEngine(GameContext context, CombatEngine combat)
    {
        _context = context;
        _combat = combat;

        //TODO: Find the right place to put this, only works specifically for goblin heckle at the moment
        //foreach (var monster in _context.Characters.Where(c => c is Monster).ToList())
        //{
        //    if (monster.Abilities.Count == 0)
        //    {
        //        monster.Abilities.Add(_context.Abilities.Where(c => c.Name == "Heckle").FirstOrDefault() as MonsterAbility);
        //        _context.SaveChanges();
        //    }
        //}

    }

    public void BeginAdventure()
    {
        //Main game logic goes here, using other methods to handle specific actions like moving, combat, etc.

        var player = ChooseAdventurer();
        bool playing = true;

        while (playing)
        {
            DisplayCurrentRoom(player);
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

        //TODO: Hack fix for testing combat, review later for better way to select monsters for combat

        //List<Monster> monsters = _context.Characters.Where(c => c is Monster).Cast<Monster>().ToList();

        //CombatEngine combat = new CombatEngine();

        //var enemy = monsters[random.Next(2)];

        //combat.StartCombat(selectedPlayer, enemy);
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