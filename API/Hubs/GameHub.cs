using DLL.Entities;
using DLL.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Erdogan_Backend.Hubs;

public class GameHub : Hub
{
    private readonly IPlayerService _playerService;
    private static readonly Dictionary<string, string> _connections = new();

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, _connections[Context.ConnectionId]);

        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task JoinGame(int gamePin)
    {
        string groupName = $"game_{gamePin}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        
        _connections[Context.ConnectionId] = groupName;
    }
}