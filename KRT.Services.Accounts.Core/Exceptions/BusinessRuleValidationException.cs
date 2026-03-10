namespace KRT.Services.Accounts.Core.Exceptions;

public class BusinessRuleValidationException : Exception
{
    public BusinessRuleValidationException() { }

    public BusinessRuleValidationException(string message)
        : base(message)
    {
    }

    public BusinessRuleValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
