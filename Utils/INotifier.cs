using InventorySyetem1.Models;

namespace InventorySyetem1.Utils;

public interface INotifier
{
    void SendNotification(string recipient, string message);
    // void SendAlarm(string recipient);//庫存不足通知
}