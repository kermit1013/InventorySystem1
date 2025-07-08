using InventorySyetem1.Models;

namespace InventorySyetem1.Utils;

public class SmsNotifier: INotifier
{
    public void SendNotification(string recipient, string message)
    {
        Console.WriteLine($"發送簡訊至{recipient}: {message}");
        // 發送簡訊邏輯實作...
        // var data = new
        // {
        //     message = new
        //     {
        //         to = new { phone_number = "5558675309" },
        //         template = "MDW2NHJ29RMTFJJ93R8H8X7Z0Y0V",
        //         data = new { recipientName = "Test" }
        //     }
        // };
        // var jsonString = JsonSerializer.Serialize(data);
        // var dataContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
    }
}