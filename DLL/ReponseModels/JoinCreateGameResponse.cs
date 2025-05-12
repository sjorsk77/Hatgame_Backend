using DLL.Entities;

namespace DLL.ReponseModels;

public class JoinCreateGameResponse(Game game, string token)
{
    public Game Game { get; private set; } = game;
    public string GroupName { get; private set; } = game.HubGroup;
    public string Token { get; private set; } = token;
}