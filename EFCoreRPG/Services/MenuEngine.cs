namespace EFCoreRPG.Services;

public class MenuEngine
{
    public MenuEngine()
    {
    }

    public string MainMenu()
    {
        //TODO: Add option to add ability to character and display all abilities and their effects
        Console.WriteLine("\n--- Game Menu ---");
        Console.WriteLine("1. Start Adventure");
        Console.WriteLine("2. Display Rooms");
        Console.WriteLine("3. Display Characters");
        Console.WriteLine("4. Display Items and Locations");
        Console.WriteLine("5. Add Character");
        Console.WriteLine("6. Find Character");
        Console.WriteLine("0. Exit");
        Console.Write("Enter your choice: ");

        var choice = Console.ReadLine();
        Console.WriteLine();

        return choice ?? " ";
    }

    public string AdventureMenu()
    {
        Console.WriteLine("\n--- Adventure Menu ---");
        Console.WriteLine("1. Move to another room");
        Console.WriteLine("2. Check inventory");
        Console.WriteLine("3. View character stats");
        Console.WriteLine("4. View abilities");
        Console.WriteLine("5. Rest to recover health");
        Console.WriteLine("6. Fight the monster");
        Console.WriteLine("7. Loot nearby");
        Console.WriteLine("0. Exit adventure");
        Console.Write("Enter your choice: ");
        var choice = Console.ReadLine();
        Console.WriteLine();
        return choice ?? " ";
    }

    public string MovementMenu()
    {
        return "";
    }

    public string InventoryMenu()
    {
        Console.WriteLine("\n--- Inventory Menu ---");
        Console.WriteLine("1. Show all items");
        Console.WriteLine("2. Use an item");
        Console.WriteLine("3. Equip an item");
        Console.WriteLine("4. Unequip an item");
        Console.WriteLine("0. Back");
        Console.Write("Enter your choice: ");
        var choice = Console.ReadLine();
        Console.WriteLine();
        return choice ?? " ";
    }
}