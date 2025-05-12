using DLL.Entities;
using DLL.ReponseModels;
using DLL.RequestModels;

namespace DLL.IServices;

public interface IGameService
{
    public Task<JoinCreateGameResponse> CreateGameAsync(string playerName);
    /*public Task<Game> UpdateGameScoreAsync(int userId, int newScore);*/
    public Task<JoinCreateGameResponse> JoinGameAsync(JoinGameReq request);
    /*public Task<Game> LeaveGameAsync(int gameId, int userId);*/
    /*public Task<List<Game>> GetLiveMatchesAsync();*/
}