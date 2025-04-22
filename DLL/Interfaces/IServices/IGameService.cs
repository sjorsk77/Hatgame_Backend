using DLL.Entities;
using DLL.ReponseModels;
using DLL.RequestModels;

namespace DLL.IServices;

public interface IGameService
{
    public Task<JoinCreateGameResponse> CreateGameAsync(CreateGameRequest request);
    public Task<Game> UpdateGameScoreAsync(int userId, int newScore);
    public Task<JoinCreateGameResponse> JoinGameAsync(JoinGameRequest request);
    public Task<Game> GetGameByIdAsync(int gameId);
    public Task<List<Game>> GetAllGamesAsync();
    public Task<Game> LeaveGameAsync(int gameId, int userId);
    public Task<List<Game>> GetLiveMatchesAsync();
}