namespace InventorySyetem1.Models;

public class OperationResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    //成功建構子
    public OperationResult(string message, T data)
    {
        Success = true;
        Message = message;
        Data = data;
    }
    
    //失敗建構子
    public OperationResult(string errorMessage)
    {
        Success = false;
        Message = errorMessage;
        Data = default(T); //null
    }

    public static OperationResult<T> SuccessResult(string message, T data)
    {
        return new OperationResult<T>(message, data);
    }

    public static OperationResult<T> ErrorResult(string message)
    {
        return new OperationResult<T>(message);
    }
}