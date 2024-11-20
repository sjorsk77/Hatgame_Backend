namespace DLL.RequestModels;

public class CreateGameRequest
{
    public string GameName { get; init; } = null!;
    public string PlayerName { get; init; } = null!;
    public string? Password { get; init; }
}