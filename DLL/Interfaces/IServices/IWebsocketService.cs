namespace DLL.Interfaces.IServices;

public interface IWebsocketService
{
    public void AddConnection(string connectionId, int userId);
    public int GetUserId(string connectionId);
    public void RemoveConnection(string connectionId);
}