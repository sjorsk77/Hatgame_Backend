using DLL.Entities;
using DLL.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Erdogan_Backend.Hubs;

public class GameHubClient : IGameHubClient
{
    private readonly IHubContext<GameHub> _hubContext;

    public GameHubClient(IHubContext<GameHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task BroadcastUpdatedGame(Game updatedGame)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveGame", updatedGame);
    }
}