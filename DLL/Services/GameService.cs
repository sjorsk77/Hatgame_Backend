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
    private readonly IGameHubClient _gameHubClient;
    private readonly IPlayerRepository _playerRepository;
    
    public GameService(
        IGameRepository gameRepository, 
        IGameHubClient gameHubClient, 
        IPlayerRepository playerRepository)
    {
        _gameRepository = gameRepository;
        _gameHubClient = gameHubClient;
        _playerRepository = playerRepository;
    }

    public async Task<Game> CreateGameAsync(CreateGameRequest request)
    {
        var host = new Player
        {
            Name = request.PlayerName,
            IsHost = true
        };
        
        var createdHost = await _playerRepository.CreatePlayerAsync(host);

        var newGame = new Game
        {
            Players = new List<Player> {createdHost},
            Name = request.GameName,
            Password = request.Password
        };
        
        var createdGame = await _gameRepository.CreateGameAsync(newGame);

        return createdGame;
    }

    public async Task<Game> UpdateGameScoreAsync(int userId, int newScore)
    {
        var updatedGame = await _gameRepository.UpdateGameScoreAsync(userId, newScore);
        
        await _gameHubClient.BroadcastUpdatedGame(updatedGame);
        
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
}