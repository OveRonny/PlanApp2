namespace Server_api.Infrastructure.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }
}
