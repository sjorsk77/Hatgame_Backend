using DLL.Entities;
using DLL.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Erdogan_Backend.Hubs;

public class GameHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var gameId = Context.GetHttpContext().Request.Query["gameId"];
        if (!string.IsNullOrEmpty(gameId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Game-{gameId}");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var gameId = Context.GetHttpContext().Request.Query["gameId"];
        if (!string.IsNullOrEmpty(gameId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Game-{gameId}");
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task JoinGame(string gameId)
    {
        if (string.IsNullOrEmpty(gameId))
        {
            throw new ArgumentException("Game ID cannot be null or empty", nameof(gameId));
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        
        await Clients.Group(gameId).SendAsync("ReceiveGroupMessage", $"{Context.ConnectionId} has joined the game.");
    }

    public async Task LeaveGame(string gameId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
        await Clients.Group(gameId).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the game {gameId}.");
    }

    public async Task SendMessageToGame(string gameId, string message)
    {
        await Clients.Group(gameId).SendAsync("ReceiveMessage", message);
    }

    public async Task BroadcastMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}