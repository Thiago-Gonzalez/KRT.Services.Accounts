namespace KRT.Services.Accounts.Core.Events;

public class AccountDeleted : IDomainEvent
{
    public AccountDeleted(int id)
    {
        Id = id;
    }

    /// <summary>
    /// Id.
    /// </summary>
    public int Id { get; private set; }
}
