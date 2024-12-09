using DLL.Entities;
using DLL.RequestModels;

namespace DLL.IServices;

public interface IGameService
{
    public Task<Game> CreateGameAsync(CreateGameRequest request);
    public Task<Game> UpdateGameScoreAsync(int userId, int newScore);
    public Task<Game> JoinGameAsync(JoinGameRequest request);
    public Task<Game> GetGameByIdAsync(int gameId);
    public Task<List<Game>> GetAllGamesAsync();
    public Task<Game> LeaveGameAsync(int playerId);
}