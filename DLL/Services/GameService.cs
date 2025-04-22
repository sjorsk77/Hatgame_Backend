using DLL.Entities;
using DLL.Interfaces;
using DLL.IRepositories;
using DLL.IServices;
using DLL.ReponseModels;
using DLL.RequestModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DLL.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ITokenService _tokenService;
    
    public GameService(
        IGameRepository gameRepository, 
        IPlayerRepository playerRepository,
        ITokenService tokenService)
    {
        _gameRepository = gameRepository;
        _playerRepository = playerRepository;
        _tokenService = tokenService;
    }

    public async Task<JoinCreateGameResponse> CreateGameAsync(CreateGameRequest request)
    {
        var host = new Player()
        {
            Name = request.PlayerName,
            IsHost = true
        };
        
        var newGame = new Game
        {
            Name = request.GameName,
            Password = request.Password,
            Players = new List<Player> { host }
        };
        
        var createdGame = await _gameRepository.CreateGameAsync(newGame);
        var token = _tokenService.GeneratePlayerToken(host);
        
        var response = new JoinCreateGameResponse
        {
            Game = createdGame,
            Token = token
        };
        
        return response;
    }

    public async Task<Game> UpdateGameScoreAsync(int userId, int newScore)
    {
        var updatedGame = await _gameRepository.UpdateGameScoreAsync(userId, newScore);
        
        return updatedGame;
    }

    public async Task<JoinCreateGameResponse> JoinGameAsync(JoinGameRequest request)
    {
        var game = await _gameRepository.GetGameByIdAsync(request.GameId);
        
        if (game.Password != null && game.Password != request.Password)
            throw new ArgumentException("Invalid password");
        
        var player = new Player
        {
            Name = request.PlayerName,
            Game = game
        };
        
        var createdPlayer = await _playerRepository.CreatePlayerAsync(player);
        
        var token = _tokenService.GeneratePlayerToken(createdPlayer);
        var updatedGame = await _gameRepository.GetGameByIdAsync(request.GameId);
        
        var response = new JoinCreateGameResponse
        {
            Game = updatedGame,
            Token = token
        };
        
        return response;
    }

    public async Task<Game> GetGameByIdAsync(int gameId)
    {
        return await _gameRepository.GetGameByIdAsync(gameId);
    }

    public async Task<List<Game>> GetAllGamesAsync()
    {
        return await _gameRepository.GetAllGamesAsync();
    }

    public Task<Game> LeaveGameAsync(int gameId, int userId)
    {
        var game = _gameRepository.GetGameByIdAsync(gameId).Result;
        
        game.Players.Remove(game.Players.FirstOrDefault(p => p.Id == userId));
        
        return _gameRepository.UpdateGameAsync(game);
    }
    
    public async Task<List<Game>> GetLiveMatchesAsync()
    {
        var games = await _gameRepository.GetAllGamesAsync();
        return await Task.FromResult(games.Where(g => g.IsLive).ToList());
    }
    
    public async Task StartGame(int gameId, string token)
    {
        var game = await _gameRepository.GetGameByIdAsync(gameId);
        if (game == null)
            throw new ArgumentException("Game not found");
        
        if (!IsHost(game, token))
            throw new ArgumentException("You are not the host of this game");
        
        game.IsLive = true;
        await _gameRepository.UpdateGameAsync(game);
    }
    
    private bool IsHost(Game game, string token)
    {
        var playerid = Int32.Parse(_tokenService.ExtractClaims(token).Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty);
        
        var gamehost = game.Players.FirstOrDefault(p => p.IsHost);
        
        return gamehost != null && gamehost.Id == playerid;
    }
}