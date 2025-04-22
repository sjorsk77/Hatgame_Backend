using System.Collections.Concurrent;
using DLL.Interfaces.IServices;

namespace DLL.Services;

public class WebsocketService : IWebsocketService
{
    private readonly ConcurrentDictionary<string, int> _connections = new();


    public void AddConnection(string connectionId, int userId)
    {
        _connections.TryAdd(connectionId, userId);
    }

    public int GetUserId(string connectionId)
    {
        return _connections.TryGetValue(connectionId, out var userId) ? userId : -1;
    }

    public void RemoveConnection(string connectionId)
    {
        _connections.TryRemove(connectionId, out _);
    }
}