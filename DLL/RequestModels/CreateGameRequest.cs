namespace DLL.RequestModels;

public class CreateGameRequest(string playerName)
{
    public string PlayerName { get; private init; } = playerName;
}