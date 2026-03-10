namespace KRT.Services.Accounts.Application.ViewModels;

public class ResultViewModel
{
    public ResultViewModel(bool isSuccess, string message, int statusCode)
    {
        IsSuccess = isSuccess;
        Message = message;
        StatusCode = statusCode;
    }

    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }
    public int StatusCode { get; private set; }

    public static ResultViewModel Success(int statusCode = 200)
        => new(true, string.Empty, statusCode);

    public static ResultViewModel Error(string message, int statusCode)
        => new(false, message, statusCode);
}

public class ResultViewModel<T> : ResultViewModel
{
    public ResultViewModel(bool isSucess, T? value, string message, int statusCode)
        : base(isSucess, message, statusCode)
    {
        Value = value;
    }

    public T? Value { get; private set; }

    public static ResultViewModel<T> Success(T value, int statusCode = 200)
        => new(true, value, string.Empty, statusCode);

    public static ResultViewModel<T> Error(string message, int statusCode, T value = default!)
        => new(false, value, message, statusCode);
}
