using DLL.Entities;

namespace DLL.Interfaces;

public interface IGameHubClient
{
    Task BroadcastUpdatedGame(Game updatedGame);
}