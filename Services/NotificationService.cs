using InventorySyetem1.Repositories;
using InventorySyetem1.Utils;

namespace InventorySyetem1.Services;

public class NotificationService
{
    private readonly INotifier _notifier;
    
    public NotificationService(INotifier notifier)
    {
        _notifier = notifier;
    }

    public void NotifyUser(string recipient, string message)
    {
        //switch-case
        //case 1 user 
        // Console.WriteLine($"準備通知用戶：{recipient}");
        //case 2 admin
        // Console.WriteLine($"準備通知管理者：{recipient}");
        Console.WriteLine($"準備通知用戶：{recipient}");
        _notifier.SendNotification(recipient, message);
        Console.WriteLine("通知操作完成。");
    }
}