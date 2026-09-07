namespace GameStoreApi.DTOs;

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;

    public List<string> Errors { get; set; } = new List<string>();
    
    public static ServiceResult<T> Success(T data, string message = "Success")
    {
        return new ServiceResult<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message,
        };
    }
    
    public static ServiceResult<T> Failure(string message, List<string>? errors = null)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            Data = default,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}