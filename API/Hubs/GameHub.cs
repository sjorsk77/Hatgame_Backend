using System.Diagnostics;
using DLL.Entities;
using DLL.Interfaces;
using DLL.IServices;
using Microsoft.AspNetCore.SignalR;

namespace Erdogan_Backend.Hubs;

public class GameHub : Hub
{
    public async Task SendGameUpdateAsync(Game game)
    {
        await Clients.All.SendAsync("ReceiveGame", game);
    }
}