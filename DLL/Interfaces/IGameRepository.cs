using DLL.Entities;

namespace DLL.IRepositories;

public interface IGameRepository
{
    public Task<Game> UpdateGameScoreAsync(int userId, int newScore);
    public Task<Game> CreateGameAsync(Game game);
    public Task<Game> GetGameByIdAsync(int gameId);
    public Task<Game> UpdateGameAsync(Game game);
}