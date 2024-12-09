using DLL.Entities;
using DLL.Interfaces;
using DLL.IRepositories;
using DLL.IServices;
using DLL.RequestModels;
using Microsoft.AspNetCore.SignalR;

namespace DLL.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IPlayerRepository _playerRepository;
    
    public GameService(
        IGameRepository gameRepository, 
        IPlayerRepository playerRepository)
    {
        _gameRepository = gameRepository;
        _playerRepository = playerRepository;
    }

    public async Task<Game> CreateGameAsync(CreateGameRequest request)
    {
        var host = new Player
        {
            Name = request.PlayerName,
        };

        var newGame = new Game
        {
            Players = new List<Player> {host},
            Name = request.GameName,
            Password = request.Password,
            Host = host
        };
        
        var createdGame = await _gameRepository.CreateGameAsync(newGame);

        return createdGame;
    }

    public async Task<Game> UpdateGameScoreAsync(int userId, int newScore)
    {
        var updatedGame = await _gameRepository.UpdateGameScoreAsync(userId, newScore);
        
        return updatedGame;
    }

    public async Task<Game> JoinGameAsync(JoinGameRequest request)
    {
        var game = await _gameRepository.GetGameByIdAsync(request.GameId);
        
        if (game.Password != null && game.Password != request.Password)
            throw new ArgumentException("Invalid password");
        
        var player = new Player
        {
            Name = request.PlayerName,
            Game = game
        };
        
        await _playerRepository.CreatePlayerAsync(player);
        
        return await _gameRepository.GetGameByIdAsync(request.GameId);
    }

    public async Task<Game> GetGameByIdAsync(int gameId)
    {
        return await _gameRepository.GetGameByIdAsync(gameId);
    }

    public async Task<List<Game>> GetAllGamesAsync()
    {
        return await _gameRepository.GetAllGamesAsync();
    }

    public Task<Game> LeaveGameAsync(int playerId)
    {
        throw new NotImplementedException();
    }
}