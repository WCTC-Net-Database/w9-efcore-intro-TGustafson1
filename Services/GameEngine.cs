using Microsoft.EntityFrameworkCore;
using W09.Data;
using W09.Models;

namespace W09.Services;

public class GameEngine
{
    private readonly GameContext _context;

    public GameEngine(GameContext context)
    {
        _context = context;
    }

    public void AddRoom()
    {
        Console.Write("Enter room name: ");
        var name = Console.ReadLine();
        Console.Write("Enter room description: ");
        var description = Console.ReadLine();
        var room = new Room { Name = name, Description = description };
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
        var rooms = _context.Rooms.Include(r => r.Characters).ToList();

        foreach (var room in rooms)
        {
            Console.WriteLine($"Room: {room.Name} - {room.Description}");
            foreach (var character in room.Characters)
            {
                Console.WriteLine($"    Character: {character.Name}, Level: {character.Level}");
            }
        }
    }

    public void DisplayCharacters()
    {
        var characters = _context.Characters.ToList();
        if (characters.Any())
        {
            Console.WriteLine("\nCharacters:");
            foreach (var character in characters)
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
}