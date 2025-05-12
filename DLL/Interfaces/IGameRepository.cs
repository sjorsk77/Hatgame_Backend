using DLL.Entities;

namespace DLL.IRepositories;

public interface IGameRepository
{
    public Task<Game> CreateGameAsync(Game game);
    public Task<Game> GetGameByIdAsync(int gameId);
    public Task<Game> GetGameByPinAsync(int gamePin);
    public Task<bool> GamePinExistsAsync(int gamePin);
}