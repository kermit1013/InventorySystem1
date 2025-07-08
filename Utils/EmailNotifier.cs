namespace InventorySyetem1.Utils;

public class EmailNotifier : INotifier
{

    public void SendNotification(string recipient, string message)
    {
        Console.WriteLine($"發送Email至{recipient}: {message}");

    }
}