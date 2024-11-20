namespace DLL.RequestModels;

public class JoinGameRequest
{
    public int GameId { get; init; }
    public string PlayerName { get; init; } = null!;
    public string? Password { get; init; }
}