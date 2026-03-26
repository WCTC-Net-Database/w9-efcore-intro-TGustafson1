namespace W09.Services;

public class Menu
{
    private readonly GameEngine _gameEngine;

    public Menu(GameEngine gameEngine)
    {
        _gameEngine = gameEngine;
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine("1. Display Rooms");
            Console.WriteLine("2. Display Characters");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _gameEngine.DisplayRooms();
                    break;
                case "2":
                    _gameEngine.DisplayCharacters();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }

}