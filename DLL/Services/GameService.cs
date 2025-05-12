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

    public async Task<JoinCreateGameResponse> CreateGameAsync(string playerName)
    {
        //create unique gamepin
        var gamePin = await GenerateUniquePinAsync();
        //generate a hub group name
        var hubGroup = $"game_{gamePin}";
        //create the game
        var game = new Game(hubGroup, gamePin);
        //create the host
        var host = new Player(playerName, game, true);
        //add the host to the game
        game.Players.Add(host);
        //save the game to db
        var createdGame = await _gameRepository.CreateGameAsync(game);
        //generate a token for the host
        var token = _tokenService.GeneratePlayerToken(host);
        //return the game and the token
        return new JoinCreateGameResponse(createdGame, token);
    }

    /*public async Task<Game> UpdateGameScoreAsync(int userId, int newScore)
    {
        var updatedGame = await _gameRepository.UpdateGameScoreAsync(userId, newScore);
        
        return updatedGame;
    }*/

    public async Task<JoinCreateGameResponse> JoinGameAsync(JoinGameReq request)
    {
        //find the game with the given pin
        var game = await _gameRepository.GetGameByPinAsync(request.GamePin);
        //check if the game is live
        if (!game.IsLive)
            throw new Exception("Game is not live");
        //check if the playername is already taken
        if (game.Players.Any(p => p.Name == request.PlayerName))
            throw new Exception("Playername already taken");
        //add the player to the game
        var player = new Player(request.PlayerName, game);
        await _playerRepository.CreatePlayerAsync(player);
        //generate a token for the player and update game var
        var token = _tokenService.GeneratePlayerToken(player);
        var updatedGame = await _gameRepository.GetGameByIdAsync(game.Id);
        //return the game and the token
        return new JoinCreateGameResponse(updatedGame, token);
    }

    /*public Task<Game> LeaveGameAsync(int gameId, int userId)
    {
        var game = _gameRepository.GetGameByIdAsync(gameId).Result;
        
        game.Players.Remove(game.Players.FirstOrDefault(p => p.Id == userId));
        
        return _gameRepository.UpdateGameAsync(game);
    }*/
    
    /*public async Task StartGame(int gameId, string token)
    {
        var game = await _gameRepository.GetGameByIdAsync(gameId);
        if (game == null)
            throw new ArgumentException("Game not found");
        
        if (!IsHost(game, token))
            throw new ArgumentException("You are not the host of this game");
        
        game.IsLive = true;
        await _gameRepository.UpdateGameAsync(game);
    }*/
    
    /*private bool IsHost(Game game, string token)
    {
        var playerid = Int32.Parse(_tokenService.ExtractClaims(token).Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty);
        
        var gamehost = game.Players.FirstOrDefault(p => p.IsHost);
        
        return gamehost != null && gamehost.Id == playerid;
    }*/
    private async Task<int> GenerateUniquePinAsync()
    {
        var random = new Random();
        int pin;
        
        do
        {
            pin = random.Next(100000, 999999);
        } while (await _gameRepository.GamePinExistsAsync(pin));
        return pin;
    }
}