namespace GameStoreApi.DTOs;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;

    public List<string> Errors { get; set; } = new List<string>();
    
    public static ApiResponse<T> Success(T data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message,
        };
    }
    
    public static ApiResponse<T> Failure(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Data = default,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}